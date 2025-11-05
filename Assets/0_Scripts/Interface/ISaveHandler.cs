public interface ISaveHandler
{
    void RegisterSaveHandler() { Managers.Save.Register(this); }
    void UnregisterSaveHandler() { Managers.Save.Unregister(this); }
    bool OnSaveRequest(GlobalDTO dto);
    bool OnLoadRequest(GlobalDTO dto);
}