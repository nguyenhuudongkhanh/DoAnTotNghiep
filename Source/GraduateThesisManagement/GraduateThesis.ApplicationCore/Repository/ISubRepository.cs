using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;

namespace GraduateThesis.ApplicationCore.Repository;

/// <summary>
/// Author: phanxuanchanh.com (phanchanhvn)
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
/// <typeparam name="T_ID"></typeparam>

public interface ISubRepository<TInput, TOutput, T_ID>
    where TInput : class 
    where TOutput : class 
{
    Pagination<TOutput> GetPagination(int page, int pageSize, string orderBy, OrderOptions orderOptions, string keyword);
    Pagination<TOutput> GetPagination(Pagination<TOutput> pagination);
    List<TOutput> GetList(int count);
    List<TOutput> GetTrash(int count);
    TOutput Get(T_ID id);
    DataResponse<TOutput> Create(TInput input);
    DataResponse<TOutput> Update(T_ID id, TInput input);
    DataResponse BatchDelete(T_ID id);
    DataResponse Restore(T_ID id);
    DataResponse ForceDelete(T_ID id);
    int Count();
    byte[] Export(RecordFilter recordFilter, ExportMetadata exportMetadata);
    DataResponse Import(Stream stream, ImportMetadata importMetadata);
}
