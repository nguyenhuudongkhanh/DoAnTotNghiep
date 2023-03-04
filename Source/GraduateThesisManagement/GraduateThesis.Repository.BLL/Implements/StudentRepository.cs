using GraduateThesis.ApplicationCore.Email;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.File;
using GraduateThesis.ApplicationCore.Hash;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.ExtensionMethods;
using GraduateThesis.Repository.BLL.Consts;
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

public class StudentRepository : AsyncSubRepository<Student, StudentInput, StudentOutput, string>, IStudentRepository
{
    private HufiGraduateThesisContext _context;
    private IHostingEnvironment _hostingEnvironment;
    private IEmailService _emailService;
    private IFileManager _fileManager;

    internal StudentRepository(HufiGraduateThesisContext context, IHostingEnvironment hostingEnvironment, IEmailService emailService, IFileManager fileManager)
        : base(context, context.Students)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
        _emailService = emailService;
        _fileManager = fileManager;
    }

    protected override void ConfigureIncludes()
    {
        IncludeMany(i => i.StudentClass);
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new StudentOutput
        {
            Id = s.Id,
            Surname = s.Surname,
            Name = s.Name,
            Email = s.Email,
            StudentClass = new StudentClassOutput
            {
                Id = s.StudentClass.Id,
                Name = s.StudentClass.Name
            },
            CreatedAt = s.StudentClass.CreatedAt,
            UpdatedAt = s.StudentClass.UpdatedAt
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new StudentOutput
        {
            Id = s.Id,
            Surname = s.Surname,
            Name = s.Name,
            Phone = s.Phone,
            Email = s.Email,
            Address = s.Address,
            Avatar = s.Avatar,
            Birthday = s.Birthday,
            Gender = s.Gender,
            Description = s.Description,
            StudentClassId = s.StudentClassId,
            StudentClass = new StudentClassOutput
            {
                Id = s.StudentClass.Id,
                Name = s.StudentClass.Name,
            },
            CreatedAt = s.StudentClass.CreatedAt,
            UpdatedAt = s.StudentClass.UpdatedAt,
            DeletedAt = s.StudentClass.DeletedAt
        };

        AdvancedImportSelector = s =>
        {
            Student student = new Student();

            student.Id = s[1] as string;
            student.Surname = s[2] as string;
            student.Name = s[3] as string;

            string studentClassId = s[4] as string;
            if (!_context.StudentClasses.Any(s => s.Id == studentClassId))
            {
                _context.StudentClasses.Add(new StudentClass
                {
                    Id = studentClassId,
                    Name = studentClassId,
                    CreatedAt = DateTime.Now
                });

                _context.SaveChanges();
            }

            student.StudentClassId = s[4] as string;
            student.Email = s[5] as string;
            student.Birthday = DateTime.ParseExact(s[6] as string, "dd/MM/yyyy", null);
            student.Password = "default";
            student.Salt = "default";
            student.CreatedAt = DateTime.Now;

            return student;
        };
    }

    protected override void SetOutputMapper(Student entity, StudentOutput output)
    {
        output.Id = entity.Id;
        output.Surname = entity.Surname;
        output.Name = entity.Name;
        output.Email = entity.Email;
    }

    protected override void SetMapperToUpdate(StudentInput input, Student entity)
    {
        entity.Surname = input.Surname;
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.Email = input.Email;
        entity.Phone = input.Phone;
        entity.Address = input.Address;
        entity.Birthday = input.Birthday;
        entity.StudentClassId = input.StudentClassId;
        entity.UpdatedAt = DateTime.Now;
    }

    protected override void SetMapperToCreate(StudentInput input, Student entity)
    {
        entity.Id = input.Id;
        entity.Surname = input.Surname;
        entity.Name = input.Name;
        entity.Description = input.Description;
        entity.Email = input.Email;
        entity.Phone = input.Phone;
        entity.Address = input.Address;
        entity.Birthday = input.Birthday;
        entity.StudentClassId = input.StudentClassId;
        entity.Password = "default";
        entity.Salt = "default";
        entity.CreatedAt = DateTime.Now;
    }

    protected override async Task<DataResponse<StudentOutput>> ValidateOnCreateAsync(StudentInput input)
    {
        bool checkExists = false;
        if (string.IsNullOrEmpty(input.Phone))
            checkExists = await _context.Students.AnyAsync(s => s.Id == input.Id || s.Email == input.Email);
        else
            checkExists = await _context.Students.AnyAsync(s => s.Id == input.Id || s.Email == input.Email || s.Phone == input.Phone);

        if (checkExists)
            return new DataResponse<StudentOutput>
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Thông tin bị trùng, vui lòng kiểm tra lại mã, địa chỉ email, SĐT của sinh viên!"
            };

        return new DataResponse<StudentOutput> { Status = DataResponseStatus.Success };
    }

    protected override async Task<DataResponse<StudentOutput>> ValidateOnUpdateAsync(StudentInput input)
    {
        bool checkExists = false;
        if (string.IsNullOrEmpty(input.Phone))
            checkExists = await _context.Students.AnyAsync(s => s.Email == input.Email && s.Id != input.Id);
        else
            checkExists = await _context.Students.AnyAsync(s => (s.Email == input.Email || s.Phone == input.Phone) && s.Id != input.Id);

        if (checkExists)
            return new DataResponse<StudentOutput>
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Thông tin bị trùng, vui lòng kiểm tra lại mã, địa chỉ email, SĐT của sinh viên!"
            };

        return new DataResponse<StudentOutput> { Status = DataResponseStatus.Success };
    }

    public async Task<Pagination<StudentOutput>> GetPaginationAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Student> queryable = _context.Students.Where(f => f.IsDeleted == false);

        if (!string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(searchBy))
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
                case "ClassName": queryable = queryable.Where(f => f.StudentClass.Name.Contains(keyword)); break;
                case "Email": queryable = queryable.Where(f => f.Email.Contains(keyword)); break;
            }
        }

        if (string.IsNullOrEmpty(orderBy))
        {
            queryable = queryable.OrderByDescending(f => f.CreatedAt);
        }
        else if (orderOptions == OrderOptions.ASC)
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderBy(f => f.Id); break;
                case "Surname": queryable = queryable.OrderBy(f => f.Surname); break;
                case "Name": queryable = queryable.OrderBy(f => f.Name); break;
                case "ClassName": queryable = queryable.OrderBy(f => f.StudentClass.Name); break;
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
                case "ClassName": queryable = queryable.OrderByDescending(f => f.StudentClass.Name); break;
                case "Email": queryable = queryable.OrderByDescending(f => f.Email); break;
                case "CreatedAt": queryable = queryable.OrderByDescending(f => f.CreatedAt); break;
            }
        }

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();

        List<StudentOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(PaginationSelector).ToListAsync();

        return new Pagination<StudentOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public override async Task<DataResponse> ImportAsync(Stream stream, ImportMetadata importMetadata)
    {
        return await _genericRepository.ImportAsync(stream, importMetadata, new ImportSelector<Student>
        {
            AdvancedImportSpreadsheet = AdvancedImportSelector
        });
    }

    public async Task<ForgotPasswordModel> CreateNewPasswordAsync(NewPasswordModel newPasswordModel)
    {
        Student student = await _context.Students
           .Where(s => s.Email == newPasswordModel.Email && s.IsDeleted == false)
           .SingleOrDefaultAsync();

        if (student == null)
            return new ForgotPasswordModel
            {
                Status = AccountStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        student.Salt = HashFunctions.GetMD5($"{student.Id}|{student.Name}|{DateTime.Now}");
        student.Password = BCrypt.Net.BCrypt.HashPassword($"{newPasswordModel.Password}>>>{student.Salt}");

        await _context.SaveChangesAsync();

        return new ForgotPasswordModel
        {
            Status = AccountStatus.Success,
            Message = "Đã đổi mật khẩu thành công, hãy thực hiện đăng nhập lại để truy cập vào hệ thống!"
        };
    }

    public async Task<AccountVerificationModel> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel)
    {
        Student student = await _context.Students
           .Where(f => f.Email == forgotPasswordModel.Email && f.IsDeleted == false)
           .SingleOrDefaultAsync();

        if (student == null)
            return new AccountVerificationModel
            {
                Status = AccountStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        Random random = new Random();
        student.VerificationCode = random.NextString(24);
        student.CodeExpTime = DateTime.Now.AddMinutes(5);
        await _context.SaveChangesAsync();

        string mailContent = Resources.EmailResource.account_verification;
        mailContent = mailContent.Replace("@verificationCode", student.VerificationCode);

        _emailService.Send(
            student.Email,
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
        Student student = await _context.Students.FindAsync(signInModel.Code);
        if (student == null)
            return new SignInResultModel
            {
                Status = AccountStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        string passwordAndSalt = $"{signInModel.Password}>>>{student.Salt}";

        if (!BCrypt.Net.BCrypt.Verify(passwordAndSalt, student.Password))
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
        Student student = await _context.Students
            .Where(s => s.Email == accountVerificationModel.Email && s.IsDeleted == false
            ).SingleOrDefaultAsync();

        if (student == null)
            return new NewPasswordModel
            {
                Status = AccountStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        if (student.VerificationCode != accountVerificationModel.VerificationCode)
            return new NewPasswordModel
            {
                Status = AccountStatus.Failed,
                Message = "Mã xác thực không trùng khớp!",
                Email = student.Email,
            };

        DateTime currentDatetime = DateTime.Now;
        if (student.CodeExpTime < currentDatetime)
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

    public async Task<object> SearchForThesisRegAsync(string keyword)
    {
        List<string> studentIds = await _context.ThesisGroupDetails
            .Include(i => i.Student)
            .Where(gd => (gd.StatusId == GroupStatusConsts.Pending || gd.StatusId == GroupStatusConsts.Joined || gd.StatusId == GroupStatusConsts.Submitted || gd.StatusId == GroupStatusConsts.Completed) && gd.Student.IsDeleted == false)
            .Select(s => s.Student.Id).ToListAsync();

        return await _context.Students.Include(i => i.StudentClass)
            .Where(s => (s.Id.Contains(keyword) || s.Name.Contains(keyword)) && s.IsDeleted == false)
            .WhereBulkNotContains(studentIds, s => s.Id)
            .Take(50).Select(s => new
            {
                Id = s.Id,
                Surname = s.Surname,
                Name = s.Name,
                StudentClass = new
                {
                    Id = s.StudentClass.Id,
                    Name = s.StudentClass.Name
                }
            }).ToListAsync();
    }

    public async Task<object> GetForThesisRegAsync(string studentId)
    {
        return await _context.Students.Where(s => s.Id == studentId && s.IsDeleted == false)
            .Select(s => new
            {
                Id = s.Id,
                Surname = s.Surname,
                Name = s.Name,
                StudentClass = new
                {
                    Id = s.StudentClass.Id,
                    Name = s.StudentClass.Name
                }
            }).SingleOrDefaultAsync();
    }

    public async Task<DataResponse> UpdateProfileAsync(StudentInput input, FileUploadModel avtUploadModel)
    {
        Student student_fromDb = await _context.Students.FindAsync(input.Id);
        if (student_fromDb == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        student_fromDb.Email = input.Email;
        student_fromDb.Phone = input.Phone;
        student_fromDb.Address = input.Address;
        student_fromDb.Birthday = input.Birthday;
        student_fromDb.Gender = input.Gender;

        if (avtUploadModel != null)
        {
            avtUploadModel.FileName = $"student-avatar_{student_fromDb.Id}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.{_fileManager.GetExtension(avtUploadModel.ContentType)}";

            _fileManager.SetPath(Path.Combine(_hostingEnvironment.WebRootPath, "avatar", "student"));
            _fileManager.Save(avtUploadModel);

            student_fromDb.Avatar = $"student/{avtUploadModel.FileName}";
        }

        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Cập nhật thông tin cá nhân thành công!"
        };
    }

    public async Task<DataResponse> SetDefaultAvatarAsync(string studentId)
    {
        Student student_fromDb = await _context.Students.FindAsync(studentId);
        if (student_fromDb == null)
            return new DataResponse
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy tài khoản này!"
            };

        if (student_fromDb.Gender == "Nam")
            student_fromDb.Avatar = "default-male-profile.png";
        else
            student_fromDb.Avatar = "default-female-profile.png";

        await _context.SaveChangesAsync();

        return new DataResponse
        {
            Status = DataResponseStatus.Success,
            Message = "Đã đặt ảnh đại diện mặc định thành công!"
        };
    }

    public async Task<Pagination<StudentOutput>> GetPgnOfUnRegdStdntAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<Student> queryable = _context.Students.Include(i => i.StudentClass).Where(s => s.IsDeleted == false);

        if (!string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(searchBy))
        {
            queryable = queryable.Where(s => s.Id.Contains(keyword) || s.Surname.Contains(keyword) || s.Name.Contains(keyword) || s.Email.Contains(keyword));
        }

        if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(searchBy))
        {
            switch (searchBy)
            {
                case "All": queryable = queryable.Where(s => s.Id.Contains(keyword) || s.Surname.Contains(keyword) || s.Name.Contains(keyword) || s.Email.Contains(keyword)); break;
                case "Id": queryable = queryable.Where(s => s.Id.Contains(keyword)); break;
                case "Surname": queryable = queryable.Where(s => s.Surname.Contains(keyword)); break;
                case "Name": queryable = queryable.Where(s => s.Name.Contains(keyword)); break;
                case "ClassName": queryable = queryable.Where(s => s.StudentClass.Name.Contains(keyword)); break;
                case "Email": queryable = queryable.Where(s => s.Email.Contains(keyword)); break;
            }
        }

        List<string> studentIds = await _context.ThesisGroupDetails.Include(i => i.Student)
                .Where(gd => gd.IsDeleted == false && gd.Student.IsDeleted == false)
                .Where(gd => gd.StatusId == GroupStatusConsts.Completed || gd.StatusId == GroupStatusConsts.Joined)
                .Select(s => s.StudentId).ToListAsync();

        queryable.WhereBulkNotContains(studentIds, s => s.Id);

        if (string.IsNullOrEmpty(orderBy))
        {
            queryable = queryable.OrderByDescending(f => f.CreatedAt);
        }
        else if (orderOptions == OrderOptions.ASC)
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderBy(s => s.Id); break;
                case "Surname": queryable = queryable.OrderBy(s => s.Surname); break;
                case "Name": queryable = queryable.OrderBy(s => s.Name); break;
                case "ClassName": queryable = queryable.OrderBy(s => s.StudentClass.Name); break;
                case "Email": queryable = queryable.OrderBy(s => s.Email); break;
                case "CreatedAt": queryable = queryable.OrderBy(s => s.CreatedAt); break;
            }
        }
        else
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderByDescending(s => s.Id); break;
                case "Surname": queryable = queryable.OrderByDescending(s => s.Surname); break;
                case "Name": queryable = queryable.OrderByDescending(s => s.Name); break;
                case "ClassName": queryable = queryable.OrderByDescending(s => s.StudentClass.Name); break;
                case "Email": queryable = queryable.OrderByDescending(s => s.Email); break;
                case "CreatedAt": queryable = queryable.OrderByDescending(s => s.CreatedAt); break;
            }
        }

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable.CountAsync();
        List<StudentOutput> onePageOfData = await queryable.Skip(n).Take(pageSize)
            .Select(s => new StudentOutput
            {
                Id = s.Id,
                Surname = s.Surname,
                Name = s.Name,
                Email = s.Email,
                StudentClass = new StudentClassOutput
                {
                    Id = s.StudentClass.Id,
                    Name = s.StudentClass.Name
                },
                CreatedAt = s.StudentClass.CreatedAt,
                UpdatedAt = s.StudentClass.UpdatedAt
            }).ToListAsync();

        return new Pagination<StudentOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<Pagination<StudentOutput>> GetPgnOfRegdStdntAsync(int page, int pageSize, string orderBy, OrderOptions orderOptions, string searchBy, string keyword)
    {
        IQueryable<ThesisGroupDetail> queryable = _context.ThesisGroupDetails
            .Include(i => i.StudentThesisGroup).Include(i => i.Student).Include(i => i.Student.StudentClass)
            .Where(gd => gd.IsDeleted == false && gd.Student.IsDeleted == false && gd.StudentThesisGroup.IsDeleted == false)
            .Where(gd => gd.StatusId == GroupStatusConsts.Completed || gd.StatusId == GroupStatusConsts.Joined);

        if (!string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(searchBy))
        {
            queryable = queryable.Where(gd => gd.Student.Id.Contains(keyword) || gd.Student.Surname.Contains(keyword) || gd.Student.Name.Contains(keyword) || gd.Student.Email.Contains(keyword) || gd.Student.StudentClass.Name.Contains(keyword));
        }

        if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(searchBy))
        {
            switch (searchBy)
            {
                case "All": queryable = queryable.Where(gd => gd.Student.Id.Contains(keyword) || gd.Student.Surname.Contains(keyword) || gd.Student.Name.Contains(keyword) || gd.Student.Email.Contains(keyword) || gd.Student.StudentClass.Name.Contains(keyword)); break;
                case "Id": queryable = queryable.Where(gd => gd.Student.Id.Contains(keyword)); break;
                case "Surname": queryable = queryable.Where(gd => gd.Student.Surname.Contains(keyword)); break;
                case "Name": queryable = queryable.Where(gd => gd.Student.Name.Contains(keyword)); break;
                case "ClassName": queryable = queryable.Where(gd => gd.Student.StudentClass.Name.Contains(keyword)); break;
                case "Email": queryable = queryable.Where(gd => gd.Student.Email.Contains(keyword)); break;
            }
        }

        if (string.IsNullOrEmpty(orderBy))
        {
            queryable = queryable.OrderByDescending(f => f.CreatedAt);
        }
        else if (orderOptions == OrderOptions.ASC)
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderBy(gd => gd.Student.Id); break;
                case "Surname": queryable = queryable.OrderBy(gd => gd.Student.Surname); break;
                case "Name": queryable = queryable.OrderBy(gd => gd.Student.Name); break;
                case "ClassName": queryable = queryable.OrderBy(gd => gd.Student.StudentClass.Name); break;
                case "Email": queryable = queryable.OrderBy(gd => gd.Student.Email); break;
                case "CreatedAt": queryable = queryable.OrderBy(gd => gd.Student.CreatedAt); break;
            }
        }
        else
        {
            switch (orderBy)
            {
                case "Id": queryable = queryable.OrderByDescending(gd => gd.Student.Id); break;
                case "Surname": queryable = queryable.OrderByDescending(gd => gd.Student.Surname); break;
                case "Name": queryable = queryable.OrderByDescending(gd => gd.Student.Name); break;
                case "ClassName": queryable = queryable.OrderByDescending(gd => gd.Student.StudentClass.Name); break;
                case "Email": queryable = queryable.OrderByDescending(gd => gd.Student.Email); break;
                case "CreatedAt": queryable = queryable.OrderByDescending(gd => gd.Student.CreatedAt); break;
            }
        }

        int n = (page - 1) * pageSize;
        int totalItemCount = await queryable
            .Join(
                _context.Theses.Where(t => t.IsDeleted == false),
                groudDetail => groudDetail.StudentThesisGroupId,
                thesis => thesis.ThesisGroupId,
                (groupDetail, thesis) => new StudentOutput
                {
                    Id = groupDetail.Student.Id
                }
            ).CountAsync();

        List<StudentOutput> onePageOfData = await queryable
            .Join(
                _context.Theses.Where(t => t.IsDeleted == false),
                groudDetail => groudDetail.StudentThesisGroupId,
                thesis => thesis.ThesisGroupId,
                (groupDetail, thesis) => new StudentOutput
                {
                    Id = groupDetail.Student.Id,
                    Surname = groupDetail.Student.Surname,
                    Name = groupDetail.Student.Name,
                    StudentClass = new StudentClassOutput
                    {
                        Id = groupDetail.Student.StudentClass.Id,
                        Name = groupDetail.Student.StudentClass.Name
                    },
                    Email = groupDetail.Student.Email,
                    Birthday = groupDetail.Student.Birthday,
                }
            ).Skip(n).Take(pageSize).ToListAsync();

        return new Pagination<StudentOutput>
        {
            Page = page,
            PageSize = pageSize,
            TotalItemCount = totalItemCount,
            Items = onePageOfData
        };
    }

    public async Task<byte[]> ExportAsync()
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "student_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<Student> students = await _context.Students.Include(i => i.StudentClass)
            .Where(f => f.IsDeleted == false).OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Name = "DANH SÁCH SINH VIÊN CỦA KHOA CNTT",
                Items = students.Select(s => new StudentExport
                {
                    Index = count++,
                    Id = s.Id,
                    Surname = s.Surname,
                    Name = s.Name,
                    ClassName = s.StudentClass.Name,
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

    public async Task<byte[]> ExportUnRegdStdntsAsync()
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "unregistered-student_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<string> studentIds = await _context.ThesisGroupDetails.Include(i => i.Student)
                .Where(gd => gd.IsDeleted == false && gd.Student.IsDeleted == false)
                .Where(gd => gd.StatusId == GroupStatusConsts.Completed || gd.StatusId == GroupStatusConsts.Joined)
                .Select(s => s.StudentId).ToListAsync();

            List<Student> students = await _context.Students.Include(i => i.StudentClass)
            .Where(f => f.IsDeleted == false)
            .WhereBulkNotContains(studentIds, s => s.Id)
            .OrderBy(f => f.Name).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Name = "DANH SÁCH SINH VIÊN CHƯA ĐĂNG KÝ KHÓA LUẬN - KHOA CNTT",
                Items = students.Select(s => new StudentExport
                {
                    Index = count++,
                    Id = s.Id,
                    Surname = s.Surname,
                    Name = s.Name,
                    ClassName = s.StudentClass.Name,
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

    public async Task<byte[]> ExportRegdStdntsAsync()
    {
        MemoryStream memoryStream = null;
        try
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "reports", "registered-student_export.xlsx");
            memoryStream = new MemoryStream();
            int count = 1;

            List<RegdStudentExport> regdStudents = await _context.ThesisGroupDetails
                .Include(i => i.Student).Include(i => i.Student.StudentClass)
                .Where(gd => gd.IsDeleted == false && gd.Student.IsDeleted == false)
                .Where(gd => gd.StatusId == GroupStatusConsts.Completed || gd.StatusId == GroupStatusConsts.Joined)
                .Join(
                    _context.Theses.Where(t => t.IsDeleted == false),
                    groudDetail => groudDetail.StudentThesisGroupId,
                    thesis => thesis.ThesisGroupId,
                    (groupDetail, thesis) => new RegdStudentExport
                    {
                        Id = groupDetail.Student.Id,
                        Surname = groupDetail.Student.Surname,
                        Name = groupDetail.Student.Name,
                        ClassName = groupDetail.Student.StudentClass.Name,
                        Email = groupDetail.Student.Email,
                        Birthday = groupDetail.Student.Birthday,
                        ThesisName = thesis.Name
                    }
                ).ToListAsync();

            await MiniExcel.SaveAsByTemplateAsync(memoryStream, path, new
            {
                Name = "DANH SÁCH SINH VIÊN ĐÃ ĐĂNG KÝ KHÓA LUẬN - KHOA CNTT",
                Items = regdStudents.Select(s =>
                {
                    s.Index = count++;
                    return s;
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

    public async Task<DataResponse<string>> CheckRegThesisAsync(string studentId)
    {
        Student student = await _context.Students.Include(i => i.ThesisGroupDetails)
           .Where(st => st.Id == studentId && st.IsDeleted == false).SingleOrDefaultAsync();

        if (student == null)
            return new DataResponse<string>
            {
                Status = DataResponseStatus.NotFound,
                Message = "Không tìm thấy sinh viên này!",
                Data = "NotFound"
            };

        if (student.ThesisGroupDetails.Any(gd => gd.StatusId == GroupStatusConsts.Completed))
            return new DataResponse<string>
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Không thể đăng ký khóa luận vì bạn đã hoàn thành khóa luận",
                Data = "Completed"
            };

        if (student.ThesisGroupDetails.Any(gd => gd.StatusId == GroupStatusConsts.Pending))
            return new DataResponse<string>
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Không thể đăng ký mới đề tài vì bạn đã được mời vào nhóm",
                Data = "Invited"
            };

        if (student.ThesisGroupDetails.Any(gd => gd.StatusId == GroupStatusConsts.Joined || gd.StatusId == GroupStatusConsts.Submitted))
            return new DataResponse<string>
            {
                Status = DataResponseStatus.AlreadyExists,
                Message = "Bạn đã đăng ký đề tài, không thể đăng ký thêm",
                Data = "Registered"
            };

        return new DataResponse<string>
        {
            Status = DataResponseStatus.Success,
            Message = "Bạn có thể đăng ký đề tài",
            Data = "Success"
        };
    }
}
