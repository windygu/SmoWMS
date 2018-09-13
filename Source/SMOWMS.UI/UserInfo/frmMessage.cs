using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smobiler.Core;
using Smobiler.Core.Controls;
using SMOWMS.UI.Layout;
using SMOWMS.Domain.Entity;
using SMOWMS.DTOs.Enum;
using SMOWMS.CommLib;

namespace SMOWMS.UI.UserInfo
{
    partial class frmMessage : Smobiler.Core.Controls.MobileForm
    {
        #region "definition"
        AutofacConfig autofacConfig = new AutofacConfig();     //����������
        public String UserID;        //�û���
        public EuserInfo eInfo;            //�û��޸���
        public Boolean isDemo;       //�Ƿ�����ʾ�˺�
        private EditUserInfoLayout Dialog = new EditUserInfoLayout();     //�޸���Ϣ
        #endregion
        /// <summary>
        /// ҳ���ʼ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMessage_Load(object sender, EventArgs e)
        {
            try
            {
                // ���ò˵���Ĭ��ѡ����
                menuToolbar.SelectedIndex = 4;

                UserID = Client.Session["UserID"].ToString();
                coreUser UserData = autofacConfig.coreUserService.GetUserByID(UserID);
                if (UserData.USER_SEX != null)
                {
                    if (Convert.ToInt32(UserData.USER_SEX) == 0)
                        btnSex.Text = "��   >";
                    else
                        btnSex.Text = "Ů   >";
                }
                if (UserData.USER_IMAGEID == null)
                {
                    if (Convert.ToInt32(UserData.USER_SEX) == 0)
                        imgUser.ResourceID = "male";
                    else
                        imgUser.ResourceID = "female";
                }
                else
                {
                    imgUser.ResourceID = UserData.USER_IMAGEID;
                }
                if (UserData.USER_ADDRESS != null) txtAddress.Text = UserData.USER_ADDRESS;
                if (UserData.USER_EMAIL != null) lblEmail.Text = UserData.USER_EMAIL;
                lblID.Text = UserID;
                if (UserData.USER_NAME != null)
                {
                    lblName.Text = UserData.USER_NAME;
                }
                else
                {
                    lblName.Text = UserID;
                }
                if (UserData.USER_PHONE != null) lblPhone.Text = UserData.USER_PHONE;
                if (UserData.USER_EMAIL != null) lblEmail.Text = UserData.USER_EMAIL;
                if (UserData.USER_BIRTHDAY != null) dpkBirthday.Value = (DateTime)UserData.USER_BIRTHDAY;

                if (UserData.USER_ISDEMO != null)
                {
                    if (UserData.USER_ISDEMO == 1)
                        isDemo = true;
                    else
                        isDemo = false;
                }
            }
            catch (Exception ex)
            {
                Toast(ex.Message);
            }
        }
        /// <summary>
        /// ͷ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgUser_Press(object sender, EventArgs e)
        {
            Camera1.GetPhoto();
        }
        /// <summary>
        /// ͼƬ�����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Camera1_ImageCaptured(object sender, BinaryResultArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(e.error))
                {
                    e.SaveFile(UserID + ".png");   //����ͼƬ�ļ���������
                    imgUser.ResourceID = UserID;
                    imgUser.Refresh();       //ˢ�µ�ǰ��ʾ����
                    UpdateUserInfo(EuserInfo.�޸�ͷ��, imgUser.ResourceID);
                }
            }
            catch (Exception ex)
            {
                Toast(ex.Message);
            }
        }
        /// <summary>
        /// �޸��ǳ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_Press(object sender, EventArgs e)
        {
            eInfo = EuserInfo.�޸��ǳ�;
            ShowDialog(Dialog);
        }
        /// <summary>
        /// �޸��Ա�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSex_Press(object sender, EventArgs e)
        {
            popSex.ShowDialog();
        }
        /// <summary>
        /// ѡ�����Ա�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popSex_Selected(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(popSex.Selection.Text) == false)
                {
                    if (btnSex.Text != popSex.Selection.Text + "   >")
                    {
                        btnSex.Text = popSex.Selection.Text + "   >";
                        UpdateUserInfo(EuserInfo.�޸��Ա�, popSex.Selection.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Toast(ex.Message);
            }
        }
        /// <summary>
        /// �޸ĵ�ַ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLocation_TouchLeave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtAddress.Text))
                UpdateUserInfo(EuserInfo.�޸ĵ�ַ, "");
            else
                UpdateUserInfo(EuserInfo.�޸ĵ�ַ, txtAddress.Text);
        }
        /// <summary>
        /// ѡ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dpkBirthday_ValueChanged(object sender, EventArgs e)
        {
            UpdateUserInfo(EuserInfo.�޸�����, dpkBirthday.Value.ToString());
        }
        /// <summary>
        /// �˻�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMessage_Press(object sender, EventArgs e)
        {
            try
            {
                if (UserID == "13123456789") throw new Exception("�����û�û�д�Ȩ��");
                frmSet frm = new frmSet();
                frm.eInfo = eInfo;
                frm.isDemo = isDemo;
                this.Show(frm);
            }
            catch (Exception ex)
            {
                Toast(ex.Message);
            }
        }
        /// <summary>
        /// �����û���Ϣ
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Value"></param>
        public void UpdateUserInfo(EuserInfo Type, String Value)
        {
            try
            {
                coreUser UserInfo = new coreUser();
                UserInfo.USER_ID = UserID;
                switch (Type)
                {
                    case EuserInfo.�޸ĵ�ַ:
                        UserInfo.USER_ADDRESS = Value;
                        break;
                    case EuserInfo.�޸�ͷ��:
                        UserInfo.USER_IMAGEID = Value;
                        break;
                    case EuserInfo.�޸��Ա�:
                        UserInfo.USER_SEX = Convert.ToInt32(Value);
                        break;
                    case EuserInfo.�޸��ǳ�:
                        UserInfo.USER_NAME = Value;
                        break;
                    case EuserInfo.�޸�����:
                        UserInfo.USER_BIRTHDAY = Convert.ToDateTime(Value);
                        break;
                    case EuserInfo.�޸���������:
                        UserInfo.USER_LOCATIONID = Value;
                        break;
                }
                ReturnInfo RInfo = autofacConfig.coreUserService.UpdateUser(UserInfo, Type);
                if (RInfo.IsSuccess)
                {
                    Toast("�޸���Ϣ�ɹ�!");
                }
                else
                {
                    throw new Exception(RInfo.ErrorInfo);
                }
            }
            catch (Exception ex)
            {
                Toast(ex.Message);
            }
        }
        /// <summary>
        /// �ֻ��Դ����ؼ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMessage_KeyDown(object sender, KeyDownEventArgs e)
        {
            if (e.KeyCode == KeyCode.Back)
                Client.Exit();
        }
        /// <summary>
        /// ע���û�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Press(object sender, EventArgs e)
        {
            MessageBox.Show("�Ƿ�ע����ǰ�û���", "ϵͳ����", MessageBoxButtons.OKCancel, (object sender1, MessageBoxHandlerArgs args) =>
            {
                if (args.Result == ShowResult.OK)
                    Client.ReStart();
            });
        }
    }
}