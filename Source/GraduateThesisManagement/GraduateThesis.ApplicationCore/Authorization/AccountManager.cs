using GraduateThesis.ApplicationCore.Context;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.Session;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

#nullable disable

namespace GraduateThesis.ApplicationCore.Authorization;

public class AccountManager: IAccountManager
{
    private readonly AppDbContext _context;
    private HttpContext _httpContext;

    public AccountManager(AppDbContext context) 
    {
        _context = context;
    }


    public AccountSession GetSession()
    {
        SessionManager sessionManager = new SessionManager(_httpContext);
        return sessionManager.GetSession<AccountSession>("account-session");
    }

    public TUser GetUser<TUser>()
    {
        AccountSession accountSession = GetSession();
        if(accountSession == null)
            throw new Exception("Session must not be null!");

        return JsonConvert.DeserializeObject<TUser>(accountSession.AccountModel.ToString());
    }

    public object GetUser(Type type)
    {
        AccountSession accountSession = GetSession();
        if (accountSession == null)
            throw new Exception("Session must not be null!");

        return JsonConvert.DeserializeObject(accountSession.AccountModel.ToString(), type);
    }

    public string GetUserId()
    {
        AccountSession accountSession = GetSession();
        if (accountSession == null)
            throw new Exception("Session must not be null!");

        return accountSession.UserId;
    }

    public void RemoveSession()
    {
        SessionManager sessionManager = new SessionManager(_httpContext);
        sessionManager.RemoveSession("account-session");
    }

    public void SetHttpContext(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }

    public void SetSession(AccountSession accountSession)
    {
        if (accountSession == null)
            throw new Exception("'AccountSession' must not be null!");

        SessionManager sessionManager = new SessionManager(_httpContext);
        sessionManager.SetSession<AccountSession>("account-session", accountSession);
    }
}
