namespace CoreHub.Web.Services;

public class SetupService
{
    private bool _isSetupCompleted = false;
    
    public bool IsSetupCompleted 
    { 
        get => _isSetupCompleted;
        set => _isSetupCompleted = value;
    }
    
    public event Action? OnSetupStateChanged;
    
    public void CompleteSetup()
    {
        _isSetupCompleted = true;
        OnSetupStateChanged?.Invoke();
    }
    
    public void ResetSetup()
    {
        _isSetupCompleted = false;
        OnSetupStateChanged?.Invoke();
    }
}
