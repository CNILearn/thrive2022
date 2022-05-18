namespace ChatViewModels;

public interface IMessageDialog
{
    Task ShowMessageAsync(string message);
}
