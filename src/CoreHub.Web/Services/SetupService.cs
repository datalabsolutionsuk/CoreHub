namespace CoreHub.Web.Services;

public class SetupService
{
    private bool _isSetupCompleted = false;
    private string? _setupToken = null;
    private DateTime? _tokenExpiry = null;
    private const int TOKEN_EXPIRY_MINUTES = 30;
    
    public bool IsSetupCompleted 
    { 
        get => _isSetupCompleted;
        set => _isSetupCompleted = value;
    }
    
    public event Action? OnSetupStateChanged;
    
    public void CompleteSetup()
    {
        _isSetupCompleted = true;
        InvalidateToken(); // Clear token after setup
        OnSetupStateChanged?.Invoke();
    }
    
    public void ResetSetup()
    {
        _isSetupCompleted = false;
        InvalidateToken();
        OnSetupStateChanged?.Invoke();
    }
    
    // Token-based access control for setup page
    public string GenerateSetupToken()
    {
        _setupToken = Guid.NewGuid().ToString();
        _tokenExpiry = DateTime.UtcNow.AddMinutes(TOKEN_EXPIRY_MINUTES);
        return _setupToken;
    }
    
    public bool ValidateSetupToken()
    {
        if (string.IsNullOrEmpty(_setupToken) || _tokenExpiry == null)
            return false;
            
        if (DateTime.UtcNow > _tokenExpiry)
        {
            InvalidateToken();
            return false;
        }
        
        return true;
    }
    
    public void InvalidateToken()
    {
        _setupToken = null;
        _tokenExpiry = null;
    }
    
    public bool HasValidToken => ValidateSetupToken();
}
