public interface ISaveHandler
{
    bool OnSaveRequest(GlobalDTO dto);
    bool OnLoadRequest(GlobalDTO dto);
}