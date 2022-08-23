public interface IDrawableUI
{
    int UIDrawOrder { get; }
    void UIDraw(int zoom);
}