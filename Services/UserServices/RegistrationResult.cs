namespace CC_Karriarpartner.Services.UserServices
{
    public enum RegistrationResult
    {
        Success,
        EmailAlreadyExists,
        InvalidEmail,
        InvalidPassword,
        Error,
        InvalidName
    }
}
