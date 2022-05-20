using System.Threading.Tasks;

namespace WinUIStreamingClient.Activation;

// For more information on understanding activation flow see
// https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/activation.md
//
// Extend this class to implement new ActivationHandlers
public abstract class ActivationHandler<T> : IActivationHandler
    where T : class
{
    // Override this method to add the activation logic in your activation handler
    protected abstract Task HandleInternalAsync(T args);

    public async Task HandleAsync(object args)
    {
        if (args is T a)
        {
            await HandleInternalAsync(a);
        }
    }

    public bool CanHandle(object args)
    {
        // CanHandle checks the args is of type you have configured
        return args is T a && CanHandleInternal(a);
    }

    // You can override this method to add extra validation on activation args
    // to determine if your ActivationHandler should handle this activation args
    protected virtual bool CanHandleInternal(T args)
    {
        return true;
    }
}
