using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Login
{
    public class HUDNotify
    {
        public const string NotifyEmptyIdField = "Please enter your Id.";
        public const string NotifyEmptyPwField = "Please enter your Password.";
        public const string NotifyEmptyPwConfirmField = "Please enter password confirm.";
        public const string NotifyEmptyNicknameField = "Please enter your nickname.";
        public const string NotifyPwMismatch = "Password and Password confirm\nis not same.";
        public const string NotifyInvalidIdForm = "Invalid Id form. Enter it like\n'ooo@ooooo.ooo'";
        public const string NotifyInvalidAccount = "Id or Password do not match.";
        public const string NotifyDuplicateId = "Input id already exist.";
        public const string NotifyDuplicateNickname = "Input nickname already exist.";
    }   
}
