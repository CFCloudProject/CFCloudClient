using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.BackgroundWorks
{
    public class FileUpdaterProcesser
    {
        private Models.FileChangeEvent e;

        public FileUpdaterProcesser(Models.FileChangeEvent e)
        {
            this.e = e;
        }

        public void run()
        {
            switch (e.Type)
            {
                case Models.FileChangeEvent.FileChangeType.ClientCreate:
                    ClientCreate(); break;
                case Models.FileChangeEvent.FileChangeType.ClientChange:
                    ClientChange(); break;
                case Models.FileChangeEvent.FileChangeType.ClientRename:
                    ClientRename(); break;
                case Models.FileChangeEvent.FileChangeType.ClientRenameAndClientChange:
                    ClientRename(); ClientChange(); break;
                case Models.FileChangeEvent.FileChangeType.ClientRenameAndServerChange:
                    ClientRename(); ServerChange(); break;
                case Models.FileChangeEvent.FileChangeType.ClientDelete:
                    ClientDelete(); break;
                case Models.FileChangeEvent.FileChangeType.ServerCreate:
                    ServerCreate(); break;
                case Models.FileChangeEvent.FileChangeType.ServerChange:
                    ServerChange(); break;
                case Models.FileChangeEvent.FileChangeType.ServerRename:
                    ServerRename(); break;
                case Models.FileChangeEvent.FileChangeType.ServerRenameAndClientChange:
                    ServerRename(); ClientChange(); break;
                case Models.FileChangeEvent.FileChangeType.ServerRenameAndServerChange:
                    ServerRename(); ServerChange(); break;
                case Models.FileChangeEvent.FileChangeType.ServerDelete:
                    ServerDelete(); break;
            }

            FileProcessing.Remove(e.ProcessingPath());
            Util.Global.updater.ThreadEnd();
        }

        private void ClientCreate()
        {

        }

        private void ClientChange()
        {

        }

        private void ClientRename()
        {

        }

        private void ClientDelete()
        {

        }

        private void ServerCreate()
        {

        }

        private void ServerChange()
        {

        }

        private void ServerRename()
        {

        }

        private void ServerDelete()
        {

        }
    }
}
