using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.BackgroundWorks
{
    public class FileChangeQueue
    {
        private List<Models.FileChangeEvent> EventQueue = new List<Models.FileChangeEvent>();

        public void Add(Models.FileChangeEvent e)
        {
            lock (EventQueue)
            {
                if (e.Type == Models.FileChangeEvent.FileChangeType.ClientCreate)
                {
                    Models.FileChangeEvent eve = EventQueue.Find((f) =>
                    {
                        return f.LocalPath.Equals(e.LocalPath);
                    });
                    if (eve == null)
                    {
                        EventQueue.Add(e);
                        return;
                    }
                    switch (eve.Type)
                    {
                        case Models.FileChangeEvent.FileChangeType.ClientDelete:
                            eve.Type = Models.FileChangeEvent.FileChangeType.ClientChange;
                            return;
                        case Models.FileChangeEvent.FileChangeType.ServerRename:
                        case Models.FileChangeEvent.FileChangeType.ServerRenameAndClientChange:
                        case Models.FileChangeEvent.FileChangeType.ServerRenameAndServerChange:
                            this.Add(new Models.FileChangeEvent(Models.FileChangeEvent.FileChangeType.ServerDelete, eve.OldLocalPath));
                            eve.Type = Models.FileChangeEvent.FileChangeType.ClientCreate;
                            return;
                        default:
                            eve.Type = Models.FileChangeEvent.FileChangeType.ClientCreate;
                            return;
                    }
                }
                else if (e.Type == Models.FileChangeEvent.FileChangeType.ClientChange)
                {
                    Models.FileChangeEvent eve1 = EventQueue.Find((f) =>
                    {
                        return f.LocalPath.Equals(e.LocalPath);
                    });
                    Models.FileChangeEvent eve2 = EventQueue.Find((f) =>
                    {
                        return (f.Type == Models.FileChangeEvent.FileChangeType.ServerRename
                        || f.Type == Models.FileChangeEvent.FileChangeType.ServerRenameAndClientChange
                        || f.Type == Models.FileChangeEvent.FileChangeType.ServerRenameAndServerChange)
                        && f.OldLocalPath.Equals(e.LocalPath);
                    });
                    if (eve2 != null)
                    {
                        eve2.Type = Models.FileChangeEvent.FileChangeType.ServerRenameAndClientChange;
                    }
                    if (eve1 != null)
                    {
                        switch (eve1.Type)
                        {
                            case Models.FileChangeEvent.FileChangeType.ClientCreate:
                                return;
                            case Models.FileChangeEvent.FileChangeType.ClientChange:
                                return;
                            case Models.FileChangeEvent.FileChangeType.ClientRename:
                            case Models.FileChangeEvent.FileChangeType.ClientRenameAndClientChange:
                            case Models.FileChangeEvent.FileChangeType.ClientRenameAndServerChange:
                                eve1.Type = Models.FileChangeEvent.FileChangeType.ClientRenameAndClientChange;
                                return;
                            default:
                                eve1.Type = Models.FileChangeEvent.FileChangeType.ClientChange;
                                return;
                        }
                    }
                    if (eve1 == null && eve2 == null)
                        EventQueue.Add(e);
                    return;
                }
                else if (e.Type == Models.FileChangeEvent.FileChangeType.ClientRename)
                {
                    Models.FileChangeEvent eve1 = EventQueue.Find((f) =>
                    {
                        return f.LocalPath.Equals(e.OldLocalPath);
                    });
                    Models.FileChangeEvent eve2 = EventQueue.Find((f) =>
                    {
                        return (f.Type == Models.FileChangeEvent.FileChangeType.ServerRename
                        || f.Type == Models.FileChangeEvent.FileChangeType.ServerRenameAndClientChange
                        || f.Type == Models.FileChangeEvent.FileChangeType.ServerRenameAndServerChange)
                        && f.OldLocalPath.Equals(e.OldLocalPath);
                    });
                    if (eve2 != null)
                    {
                        if (eve2.Type == Models.FileChangeEvent.FileChangeType.ServerRename)
                            eve2.Type = Models.FileChangeEvent.FileChangeType.ClientRename;
                        else if (eve2.Type == Models.FileChangeEvent.FileChangeType.ServerRenameAndClientChange)
                            eve2.Type = Models.FileChangeEvent.FileChangeType.ClientRenameAndClientChange;
                        else
                            eve2.Type = Models.FileChangeEvent.FileChangeType.ClientRenameAndServerChange;
                        eve2.OldLocalPath = eve2.LocalPath;
                        eve2.OldCloudPath = eve2.CloudPath;
                        eve2.LocalPath = e.LocalPath;
                        eve2.CloudPath = e.CloudPath;
                    }
                    if (eve1 != null)
                    {
                        switch (eve1.Type)
                        {
                            case Models.FileChangeEvent.FileChangeType.ClientCreate:
                                eve1.LocalPath = e.LocalPath;
                                eve1.CloudPath = e.CloudPath;
                                return;
                            case Models.FileChangeEvent.FileChangeType.ClientChange:
                                eve1.Type = Models.FileChangeEvent.FileChangeType.ClientRenameAndClientChange;
                                eve1.LocalPath = e.LocalPath;
                                eve1.CloudPath = e.CloudPath;
                                eve1.OldCloudPath = e.OldCloudPath;
                                eve1.OldLocalPath = e.OldLocalPath;
                                return;
                            case Models.FileChangeEvent.FileChangeType.ClientRename:
                            case Models.FileChangeEvent.FileChangeType.ClientRenameAndClientChange:
                            case Models.FileChangeEvent.FileChangeType.ClientRenameAndServerChange:
                                eve1.LocalPath = e.LocalPath;
                                eve1.CloudPath = e.CloudPath;
                                return;
                            case Models.FileChangeEvent.FileChangeType.ServerChange:
                                eve1.Type = Models.FileChangeEvent.FileChangeType.ClientRenameAndServerChange;
                                eve1.LocalPath = e.LocalPath;
                                eve1.CloudPath = e.CloudPath;
                                eve1.OldCloudPath = e.OldCloudPath;
                                eve1.OldLocalPath = e.OldLocalPath;
                                return;
                            case Models.FileChangeEvent.FileChangeType.ServerDelete:
                                eve1.LocalPath = e.LocalPath;
                                eve1.CloudPath = e.CloudPath;
                                return;
                            default:
                                return;
                        }
                    }
                    if (eve1 == null && eve2 == null)
                        EventQueue.Add(e);
                    return;
                }
                else if (e.Type == Models.FileChangeEvent.FileChangeType.ClientDelete)
                {
                    Models.FileChangeEvent eve = EventQueue.Find((f) =>
                    {
                        return f.LocalPath.Equals(e.LocalPath);
                    });
                    Models.FileChangeEvent eve2 = EventQueue.Find((f) =>
                    {
                        return (f.Type == Models.FileChangeEvent.FileChangeType.ServerRename
                        || f.Type == Models.FileChangeEvent.FileChangeType.ServerRenameAndClientChange
                        || f.Type == Models.FileChangeEvent.FileChangeType.ServerRenameAndServerChange) 
                        && f.OldLocalPath.Equals(e.LocalPath);
                    });
                    if (eve2 != null)
                    {
                        eve2.Type = Models.FileChangeEvent.FileChangeType.ClientDelete;
                    }
                    if (eve != null)
                    {
                        switch (eve.Type)
                        {
                            case Models.FileChangeEvent.FileChangeType.ClientCreate:
                            case Models.FileChangeEvent.FileChangeType.ServerDelete:
                                EventQueue.Remove(eve);
                                return;
                            case Models.FileChangeEvent.FileChangeType.ClientRename:
                            case Models.FileChangeEvent.FileChangeType.ClientRenameAndClientChange:
                            case Models.FileChangeEvent.FileChangeType.ClientRenameAndServerChange:
                                eve.Type = Models.FileChangeEvent.FileChangeType.ClientDelete;
                                eve.CloudPath = eve.OldCloudPath;
                                eve.LocalPath = eve.OldLocalPath;
                                return;
                            default:
                                eve.Type = Models.FileChangeEvent.FileChangeType.ClientDelete;
                                return;
                        }
                    }
                    if (eve == null && eve2 == null)
                        EventQueue.Add(e);
                    return;
                }
                else if (e.Type == Models.FileChangeEvent.FileChangeType.ServerCreate)
                {
                    Models.FileChangeEvent eve = EventQueue.Find((f) =>
                    {
                        return f.LocalPath.Equals(e.LocalPath);
                    });
                    if (eve == null)
                    {
                        EventQueue.Add(e);
                        return;
                    }
                    switch (eve.Type)
                    {
                        case Models.FileChangeEvent.FileChangeType.ClientCreate:
                            eve.Type = Models.FileChangeEvent.FileChangeType.ClientCreate;
                            return;
                        case Models.FileChangeEvent.FileChangeType.ClientChange:
                            eve.Type = Models.FileChangeEvent.FileChangeType.ClientChange;
                            return;
                        case Models.FileChangeEvent.FileChangeType.ServerDelete:
                            eve.Type = Models.FileChangeEvent.FileChangeType.ServerChange;
                            return;
                        default:
                            eve.Type = Models.FileChangeEvent.FileChangeType.ServerCreate;
                            return;
                    }
                }
                else if (e.Type == Models.FileChangeEvent.FileChangeType.ServerChange)
                {
                    Models.FileChangeEvent eve = EventQueue.Find((f) =>
                    {
                        return f.LocalPath.Equals(e.LocalPath);
                    });
                    if (eve != null)
                    {
                        switch (eve.Type)
                        {
                            case Models.FileChangeEvent.FileChangeType.ClientCreate:
                                eve.Type = Models.FileChangeEvent.FileChangeType.ClientCreate;
                                return;
                            case Models.FileChangeEvent.FileChangeType.ClientChange:
                                eve.Type = Models.FileChangeEvent.FileChangeType.ClientChange;
                                return;
                            case Models.FileChangeEvent.FileChangeType.ClientRename:
                                eve.Type = Models.FileChangeEvent.FileChangeType.ClientRenameAndServerChange;
                                return;
                            case Models.FileChangeEvent.FileChangeType.ClientDelete:
                                eve.Type = Models.FileChangeEvent.FileChangeType.ServerChange;
                                return;
                            case Models.FileChangeEvent.FileChangeType.ServerRename:
                                eve.Type = Models.FileChangeEvent.FileChangeType.ServerRenameAndServerChange;
                                return;
                            default:
                                return;
                        }
                    }
                    EventQueue.Add(e);
                    return;
                }
                else if (e.Type == Models.FileChangeEvent.FileChangeType.ServerRename)
                {
                    Models.FileChangeEvent eve1 = EventQueue.Find((f) =>
                    {
                        return f.LocalPath.Equals(e.OldLocalPath);
                    });
                    Models.FileChangeEvent eve2 = EventQueue.Find((f) =>
                    {
                        return f.Type == Models.FileChangeEvent.FileChangeType.ClientCreate
                        && f.LocalPath.Equals(e.LocalPath);
                    });
                    if (eve2 != null)
                    {
                        this.Add(new Models.FileChangeEvent(Models.FileChangeEvent.FileChangeType.ServerDelete, e.OldLocalPath));
                    }
                    if (eve1 != null)
                    {
                        switch (eve1.Type)
                        {
                            case Models.FileChangeEvent.FileChangeType.ClientChange:
                                eve1.Type = Models.FileChangeEvent.FileChangeType.ServerRenameAndClientChange;
                                eve1.LocalPath = e.LocalPath;
                                eve1.CloudPath = e.CloudPath;
                                return;
                            case Models.FileChangeEvent.FileChangeType.ClientRename:
                                eve1.Type = Models.FileChangeEvent.FileChangeType.ServerRename;
                                eve1.OldCloudPath = eve1.CloudPath;
                                eve1.OldLocalPath = eve1.LocalPath;
                                eve1.CloudPath = e.CloudPath;
                                eve1.LocalPath = e.LocalPath;
                                return;
                            case Models.FileChangeEvent.FileChangeType.ClientRenameAndClientChange:
                                eve1.Type = Models.FileChangeEvent.FileChangeType.ServerRenameAndClientChange;
                                eve1.OldCloudPath = eve1.CloudPath;
                                eve1.OldLocalPath = eve1.LocalPath;
                                eve1.CloudPath = e.CloudPath;
                                eve1.LocalPath = e.LocalPath;
                                return;
                            case Models.FileChangeEvent.FileChangeType.ClientRenameAndServerChange:
                                eve1.Type = Models.FileChangeEvent.FileChangeType.ServerRenameAndServerChange;
                                eve1.OldCloudPath = eve1.CloudPath;
                                eve1.OldLocalPath = eve1.LocalPath;
                                eve1.CloudPath = e.CloudPath;
                                eve1.LocalPath = e.LocalPath;
                                return;
                            case Models.FileChangeEvent.FileChangeType.ClientDelete:
                            case Models.FileChangeEvent.FileChangeType.ServerCreate:
                            case Models.FileChangeEvent.FileChangeType.ServerRename:
                            case Models.FileChangeEvent.FileChangeType.ServerRenameAndClientChange:
                            case Models.FileChangeEvent.FileChangeType.ServerRenameAndServerChange:
                                eve1.LocalPath = e.LocalPath;
                                eve1.CloudPath = e.CloudPath;
                                return;
                            case Models.FileChangeEvent.FileChangeType.ServerChange:
                                eve1.Type = Models.FileChangeEvent.FileChangeType.ServerRenameAndServerChange;
                                eve1.LocalPath = e.LocalPath;
                                eve1.CloudPath = e.CloudPath;
                                return;
                            default:
                                return;
                        }
                    }
                    if (eve1 == null && eve2 == null)
                        EventQueue.Add(e);
                    return;
                }
                else if (e.Type == Models.FileChangeEvent.FileChangeType.ServerDelete)
                {
                    Models.FileChangeEvent eve = EventQueue.Find((f) =>
                    {
                        return f.LocalPath.Equals(e.LocalPath);
                    });
                    if (eve != null)
                    {
                        switch (eve.Type)
                        {
                            case Models.FileChangeEvent.FileChangeType.ClientCreate:
                            case Models.FileChangeEvent.FileChangeType.ClientChange:
                                return;
                            case Models.FileChangeEvent.FileChangeType.ClientRename:
                            case Models.FileChangeEvent.FileChangeType.ClientRenameAndClientChange:
                            case Models.FileChangeEvent.FileChangeType.ClientRenameAndServerChange:
                            case Models.FileChangeEvent.FileChangeType.ServerChange:
                                eve.Type = Models.FileChangeEvent.FileChangeType.ServerDelete;
                                return;
                            case Models.FileChangeEvent.FileChangeType.ClientDelete:
                            case Models.FileChangeEvent.FileChangeType.ServerCreate:
                                EventQueue.Remove(eve);
                                return;
                            case Models.FileChangeEvent.FileChangeType.ServerRename:
                            case Models.FileChangeEvent.FileChangeType.ServerRenameAndClientChange:
                            case Models.FileChangeEvent.FileChangeType.ServerRenameAndServerChange:
                                eve.Type = Models.FileChangeEvent.FileChangeType.ServerDelete;
                                eve.LocalPath = eve.OldLocalPath;
                                eve.CloudPath = eve.OldCloudPath;
                                return;
                            case Models.FileChangeEvent.FileChangeType.ServerDelete:
                            default:
                                return;
                        }
                    }
                    EventQueue.Add(e);
                    return;
                }
            }
        }

        public Models.FileChangeEvent GetAnEvent()
        {
            lock (EventQueue)
            {
                if (EventQueue.Count != 0)
                {
                    for (int i = 0; i < EventQueue.Count; ++i)
                    {
                        Models.FileChangeEvent e = EventQueue.ElementAt(i);
                        if (!FileProcessing.isInProcessing(e.ProcessingPath()))
                        {
                            EventQueue.Remove(e);
                            return e;
                        }
                    }
                    return null;
                }
                else
                {
                    Util.Global.manualResetEvent.Reset();
                    return null;
                }
            }
        }
    }
}
