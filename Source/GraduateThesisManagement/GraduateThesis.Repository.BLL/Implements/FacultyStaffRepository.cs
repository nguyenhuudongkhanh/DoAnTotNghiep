using GraduateThesis.ApplicationCore.Email;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.File;
using GraduateThesis.ApplicationCore.Hash;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ExtensionMethods;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class FacultyStaffRepository : AsyncSubRepository<FacultyStaff, FacultyStaffInput, FacultyStaffOutput, string>, IFacultyStaffRepository
{
    private HufiGraduateThesisContext _context;
    private IHostingEnvironment _hostingEnvironment;
    private IEmailService _emailService;
    private IFileManager _fileManager;

    internal FacultyStaffRepository(HufiGraduateThesisContext context, IHostingEnvironment hostingEnvironment, IEmailService emailService, IFileManager fileManager)
        : base(context, context.FacultyStaffs)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
        _emailService = emailService;
        _fileManager = fileManager;
    }

    protected override void ConfigureIncludes()
    {
        IncludeMany(i => i.Faculty);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new FacultyStaffOutput
        {
            Id = s.Id,
            Surname = s.Surname,
            Name = s.Name,
            Email = s.Email,
            Roles = _context.AppUserRoles.Include(i => i.Role)
                .Where(ur => ur.UserId == s.Id).Select(ur => new AppRoleOutput
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name
                }).ToList(),
            CreatedAt = s.CreatedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new FacultyStaffOutput
        {
            Id = s.Id,
            Surname = s.Surname,
            Name = s.Name,
            Email = s.Email,
            Phone = s.Phone,
            Address = s.Address,
            Avatar = s.Avatar,
            Birthday = s.Birthday,
            Gender = s.Gender,
            Description = s.Description,
            FacultyId = s.FacultyId,
            Faculty = new FacultyOutput
            {
                Id = s.Faculty.Id,
                Name = s.Faculty.Name
            },
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        };

        AdvancedImportSelector = s => new FacultyStaff
        {
            Id = s[1] as string,
            Surname = s[2] as string,
            Name = s[3] as string,
            Email = s[4] as string,
            Birthday = DateTime.ParseExact(s[5] as string, "dd/MM/yyyy", null),
            Password = "default",
            Salt = "default",
            CreatedAt = DateTime.Now
        };
    }

    protected override void SetOutputMapper(FacultyStaff entity, FacultyStaffOutput output)
    {
        output.Id = entity.Id;
        output.Surname = entity.Surname;
        output.Name = entity.Name;
        output.Email = entity.Email;
    }

    protected override void SetMapperToUpdate(FacultyStaffInput input, FacultyStaff entity)
    {
        entity.Surname = input.Surname;
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.Email = input.Email;
        entity.Phone = input.Phone;
        entity.Address = input.Address;
        entity.Birthday = input.Birthday;
        entity.FacultyId = input.FacultyId;
        entity.UpdatedAt = DateTime.Now;
    }

    protected override void SetMapperToCreate(FacultyStaffInput input, FacultyStaff entity)
    {
        entity.Id = input.Id;
        entity.Surname = input.Surname;
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.Email = input.Email;
        entity.Phone = input.Phone;
        entity.Address = input.Address;
        entity.Birthday = input.Birthday;
        entity.Gender = input.Gender;
        entity.FacultyId = input.FacultyId;
        entity.Password = "default";
        entity.Salt = "default";
        entity.CreatedAt = DateTime.Now;
    }

    protected override async Task<DataResponse<FacultyStaffOutput>> ValidateOnCreateAsync(FacultyStaffInput input)
    {
        bool checkExists = false;
        if (string.IsNullOrEmpty(input.Phone))
            checkExists = await _context.FacultyStaffs.AnyAsync(f => f.Id == input.Id || f.Email == input.Email);
        else
            checkExists = await _context.FacultyStaffs.AnyAsync(f => f.Id == input.Id || f.Email == input.Email || f.Phone == input.Phone);

        if (checkExists)
            return new DataResponse<FacultyStaffOutput>
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Thông tin bị trùng, vui lòng kiểm tra lại mã, địa chỉ email của giảng viên!"
            };

        return new DataResponse<FacultyStaffOutput> { Status = DataResponseStatus.Success };
    }

    protected override async Task<DataResponse<FacultyStaffOutput>> ValidateOnUpdateAsync(FacultyStaffInput input)
    {
        bool checkExists = false;
        if (string.IsNullOrEmpty(input.Phone))
            checkExists = await _context.FacultyStaffs
                .AnyAsync(f => f.Email == input.Email && f.Id != input.Id);
        else
            checkExists = await _context.FacultyStaffs
                .AnyAsync(f => (f.Email == input.Email || f.Phone == input.Phone) && f.Id != input.Id);

        if (checkExists)
            return new DataResponse<FacultyStaffOutput>
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Thông tin bị trùng, vui lòng kiểm tra lại mã, địa chỉ email, SĐT của giảng viên!"
            };

        return new DataResponse<FacultyStaffOutput> { Status = DataResponseStatus.Success };
    }

    public override async Task<FacultyStaffOutput> GetAsync(string id)
    {
        FacultyStaffOutput facultyStaff = await base.GetAsync(id);
        if(facultyStaff != null)
        {
            facultyStaff.Roles = await _context.AppUserRoles.Include(i => i.Role)
                .Where(ur => ur.UserId == id).Select(s => new AppRoleOutput
                {
                    Id = s.Role.Id,
                    Name = s.Role.Name
                }).ToListAsync();
        }

        return facultyStaff;
    }

    public async Task<Pagination<FacultyStaffOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<FacultyStaff> queryable = _context.FacultyStaffs.Where(f => f.IsDeleted == false);

        if(!string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(searchBy))
        {
            queryable = queryable.Where(f => f.Id.Contains(keyword) || f.Surname.Contains(keyword) || f.Name.Contains(keyword) || f.Email.Contains(keyword));
        }

        if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(searchBy))
        {
            switch (searchBy)
            {
                case "All": queryable = queryable.Where(f => f.Id.Contains(keyword) || f.Surname.Contains(keyword) || f.Name.Contains(keyword) || f.Email.Contains(keyword)); break;
                case "Id": queryable = queryable.Where(f => f.Id.Contains(keyword)); break;
                case "Surname": queryable = queryable.Where(f => f.Surname.Contains(keyword)); break;
                case "Name": queryable = queryable.Where(f => f.Name.Contains(keyword)); break; 
                case "Email": queryable = queryable.Where(f => f.Email.Contains(keyword)); break;
            }
        }

        if (string.IsNullOrEmpty(orderBy)){
            queryable = queryable.OrderByDescending(f => f.CreatedAt);
        }else if(orderOptions == OrderOptions.ASC)
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderBy(f => f.Id); break;
                case "Surname": queryable = queryable.OrderBy(f => f.Surname); break;
                case "Name": queryable = queryable.OrderBy(f => f.Name); break;
                case "Email": queryable = queryable.OrderBy(f => f.Email); break;
                case "CreatedAt": queryable = queryable.OrderBy(f => f.CreatedAt); break;
            }
        }
        else
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderByDescending(f => f.Id); break;
                case "Surname": queryable = queryable.OrderByDescending(f => f.Surname); break;
                case "Name": queryable = queryable.OrderByDescending(f => f.Name); break;
                case "Email": queryable = queryable.OrderByDescending(f => f.Email); break;
                case "CreatedAt": queryable = queryable.OrderByDescending(f => f.CreatedAt); break;
            }
        }

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<FacultyStaffOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(PaginationSelector).ToListAsync();

        return new Pagination<FacultyStaffOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public override async Task<DataResponse> ImportAsync(Stream stream, ImportMetadata importMetadata)
    {
        return await _genericRepository.ImportAsync(stream, importMetadata, new ImportSelector<FacultyStaff>
        {
            AdvancedImportSpreadsheet = AdvancedImportSelector
        });
    }

    public async Task<ForgotPasswordModel> CreateNewPasswordAsync(NewPasswordModel newPasswordModel)
    {
        FacultyStaff facultyStaff = await _context.FacultyStaffs
           .Where(f => f.Email == newPasswordModel.Email && f.IsDeleted == false)
           .SingleOrDefaultAsync();

        if (facultyStaff == null)
            return new ForgotPasswordModel
            {
                Status = AccountStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        facultyStaff.Salt = HashFunctions.GetMD5($"{facultyStaff.Id}|{facultyStaff.Surname}|{facultyStaff.Name}|{DateTime.Now}");
        facultyStaff.Password = BCrypt.Net.BCrypt.HashPassword($"{newPasswordModel.Password}>>>{facultyStaff.Salt}");

        await _context.SaveChangesAsync();

        return new ForgotPasswordModel
        {
            Status = AccountStatus.Success,
            Message = "Đã đổi mật khẩu thành công, hãy thực hiện đăng nhập lại để truy cập vào hệ thống!"
        };
    }

    public async Task<AccountVerificationModel> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel)
    {
        FacultyStaff facultyStaff = await _context.FacultyStaffs
           .Where(f => f.Email == forgotPasswordModel.Email && f.IsDeleted == false)
           .SingleOrDefaultAsync();

        if (facultyStaff == null)
            return new AccountVerificationModel
            {
                Status = AccountStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        Random random = new Random();
        facultyStaff.VerificationCode = random.NextString(24);
        facultyStaff.CodeExpTime = DateTime.Now.AddMinutes(5);
        await _context.SaveChangesAsync();

        string mailContent = Resources.EmailResource.account_verification;
        mailContent = mailContent.Replace("@verificationCode", facultyStaff.VerificationCode);

        _emailService.Send(
            facultyStaff.Email,
            $"Khôi phục mật khẩu [{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}]",
            mailContent
        );

        return new AccountVerificationModel
        {
            Status = AccountStatus.Success,
            Message = "Thực hiện bước 1 của quá trình lấy lại mật khẩu thành công!",
            Email = forgotPasswordModel.Email
        };
    }

    public async Task<SignInResultModel> SignInAsync(SignInModel signInModel)
    {
        FacultyStaff facultyStaff = await _context.FacultyStaffs.FindAsync(signInModel.Code);
        if (facultyStaff == null)
            return new SignInResultModel
            {
                Status = AccountStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        string passwordAndSalt = $"{signInModel.Password}>>>{facultyStaff.Salt}";

        if (!BCrypt.Net.BCrypt.Verify(passwordAndSalt, facultyStaff.Password))
            return new SignInResultModel
            {
                Status = AccountStatus.WrongPassword,
                Message = "Mật khẩu không trùng khớp!"
            };

        return new SignInResultModel
        {
            Status = AccountStatus.Success,
            Message = "Đã đăng nhập vào hệ thống thành công!"
        };
    }

    public async Task<NewPasswordModel> VerifyAccountAsync(AccountVerificationModel accountVerificationModel)
    {
        FacultyStaff facultyStaff = await _context.FacultyStaffs
            .Where(f => f.Email == accountVerificationModel.Email && f.IsDeleted == false)
            .SingleOrDefaultAsync();

        if (facultyStaff == null)
            return new NewPasswordModel
            {
                Status = AccountStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        if (accountVerificationModel.VerificationCode != facultyStaff.VerificationCode)
            return new NewPasswordModel
            {
                Status = AccountStatus.Failed,
                Message = "Mã xác thực không trùng khớp!"
            };

        DateTime currentDatetime = DateTime.Now;
        if (facultyStaff.CodeExpTime < currentDatetime)
            return new NewPasswordModel
            {
                Status = AccountStatus.Failed,
                Message = "Mã xác nhận đã hết hạn!"
            };

        return new NewPasswordModel
        {
            Status = AccountStatus.Success,
            Message = "Thực hiện bước 2 của quá trình lấy lại mật khẩu thành công!",
        };
    }

    public async Task<DataResponse> UpdateProfileAsync(FacultyStaffInput input, FileUploadModel avtUploadModel)
    {
        FacultyStaff facultyStaff_fromDb = await _context.FacultyStaffs.FindAsync(input.Id);
        if (facultyStaff_fromDb == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        facultyStaff_fromDb.Email = input.Email;
        facultyStaff_fromDb.Phone = input.Phone;
        facultyStaff_fromDb.Address = input.Address;
        facultyStaff_fromDb.Birthday = input.Birthday;
        facultyStaff_fromDb.Gender = input.Gender;

        if (avtUploadModel != null)
        {
            avtUploadModel.FileName = $"faculty-staff-avatar_{facultyStaff_fromDb.Id}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{_fileManager.GetExtension(avtUploadModel.ContentType)}";

            _fileManager.SetPath(Path.Combine(_hostingEnvironment.WebRootPath, "avatar", "faculty-staff"));
            _fileManager.Save(avtUploadModel);

            facultyStaff_fromDb.Avatar = $"faculty-staff/{avtUploadModel.FileName}";
        }

        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Cập nhật thông tin cá nhân thành công!"
        };
    }

    public async Task<DataResponse> SetDefaultAvatarAsync(string facultyStaffId)
    {
        FacultyStaff facultyStaff_fromDb = await _context.FacultyStaffs.FindAsync(facultyStaffId);
        if (facultyStaff_fromDb == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        if (facultyStaff_fromDb.Gender == "Nam")
            facultyStaff_fromDb.Avatar = "default-male-profile.png";
        else
            facultyStaff_fromDb.Avatar = "default-female-profile.png";

        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Đã đặt ảnh đại diện mặc định thành công!"
        };
    }

    public async Task<byte[]> ExportAsync()
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "faculty-staff_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<FacultyStaff> facultyStaffs = await _context.FacultyStaffs
            .Where(f => f.IsDeleted == false).OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Name = "DANH SÁCH GIẢNG VIÊN CỦA KHOA CNTT",
                Items = facultyStaffs.Select(s => new FacultyStaffExport
                {
                    Index = count++,
                    Id = s.Id,
                    Surname = s.Surname,
                    Name = s.Name,
                    Email = s.Email,
                    Birthday = s.Birthday
                }).ToList()
            });

            return memoryStream.ToArray();
        }
        finally
        {
            if (memoryStream != null)
                memoryStream.Dispose();
        }
    }

    public async Task<DataResponse> GrantRoleAsync(AppUserRoleInput input)
    {
        bool checkRolesExists = await _context.AppRoles
            .AnyAsync(r => r.Id == input.RoleId && r.IsDeleted == false);
        if (!checkRolesExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy quyền có Id này!"
            };

        bool checkUserExists = await _context.FacultyStaffs
            .AnyAsync(f => f.Id == input.UserId && f.IsDeleted == false);
        if (!checkUserExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy người dùng có Id này!"
            };

        bool checkExists = await _context.AppUserRoles
            .AnyAsync(ur => ur.RoleId == input.RoleId && ur.UserId == input.UserId);

        if (checkExists)
            return new DataResponse
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Quyền đã được gán vào cho người dùng, không thể thực hiện gán trùng!"
            };

        AppUserRole appUserRole = new AppUserRole
        {
            RoleId = input.RoleId,
            UserId = input.UserId,
            CreatedAt = DateTime.Now
        };

        await _context.AppUserRoles.AddAsync(appUserRole);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Đã gán quyền thành công!"
        };
    }

    public async Task<DataResponse> RevokeRoleAsync(AppUserRoleInput input)
    {
        bool checkRolesExists = await _context.AppRoles
            .AnyAsync(r => r.Id == input.RoleId && r.IsDeleted == false);
        if (!checkRolesExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy quyền có Id này!"
            };

        bool checkUserExists = await _context.FacultyStaffs
            .AnyAsync(f => f.Id == input.UserId && f.IsDeleted == false);
        if (!checkUserExists)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy người dùng có Id này!"
            };

        AppUserRole appUserRole = await _context.AppUserRoles
            .FindAsync(input.UserId, input.RoleId);

        if (appUserRole == null)
            return new DataResponse
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Không thể thu hồi quyền vì không tồn tại trong hệ thống!"
            };

        _context.AppUserRoles.Remove(appUserRole);
        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Đã thu hồi quyền thành công!"
        };
    }
}
