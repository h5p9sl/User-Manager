using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager
{
    public class UserInformation
    {
        public UInt64 uid;
        public string username;
        public byte[] macAddress;
        public UInt64 expiryDate;
        public string notes;

        public void SetTo(UserInformation user)
        {
            this.uid = user.uid;
            this.username = user.username;
            this.macAddress = user.macAddress;
            this.expiryDate = user.expiryDate;
        }

        public UserInformation()
        {
            this.uid = 0;
            this.username = null;
            this.macAddress = null;
            this.expiryDate = 0;
        }

        public UserInformation(ref UserManager userManager)
        {
            this.uid = 0;
            if (userManager.Count != 0)
            {
                this.uid = userManager.Last().uid + 1;
            }
            username = "User " + this.uid;
            this.macAddress = new byte[]{0, 0, 0, 0, 0, 0};
            this.expiryDate = (UInt64)DateTime.Now.Ticks;
        }
    }

    public class UserManager : List<UserInformation>
    {
        private static UserManager instance = null;
        private static readonly object padlock = new object();

        public static UserManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new UserManager();
                    }
                    return instance;
                }
            }
        }


    }


}
