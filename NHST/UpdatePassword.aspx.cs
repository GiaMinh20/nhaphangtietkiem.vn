using NHST.Bussiness;
using NHST.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST
{
    public partial class UpdatePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }


        public void LoadData()
        {
            var la = AccountController.GetAllNotSearch();
            if (la.Count > 0)
            {
                foreach (var item in la)
                {
                    if (item.Token == null)
                    {
                        string Token = PJUtils.RandomStringWithText(16);
                        var passold = PJUtils.Decrypt("userpass", item.Password);
                        AccountController.UpdatePasswordNew(item.ID, passold, Token);
                    }
                }
            }
        }
    }
}