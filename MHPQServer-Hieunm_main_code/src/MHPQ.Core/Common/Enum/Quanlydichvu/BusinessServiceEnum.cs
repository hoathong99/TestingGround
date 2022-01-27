

using System;

namespace MHPQ.Common.Enum
{
    public static partial class CommonENumObject
    {

        public enum BUSINESS
        {
            ELECTRIC_REPAIR = 1,
            WATER_REPAIR = 2,
            FURNITURE = 3,
            REFRIGERATION = 4

        }

        public enum STATE_OBJECT
        {
            NEW = 1,
            ACTIVE = 2,
            REFUSE = 3,
            DISABLE = 4

        }
        public enum FORMCASE_GET_DATA
        {
            [EnumDisplayString("Form get all data")]
             OBJECT_GETALL = 1
            ,
            [EnumDisplayString("Form get detail data")]
            OBJECT_DETAIL = 2
            ,

        }

        public enum FORM_ID_OBJECT
        {
            [EnumDisplayString("Form admin getall object")]
            FORM_ADMIN_GET_OBJECT_GETALL = 1
            ,
            [EnumDisplayString("Form admin getall object new")]
            FORM_ADMIN_GET_OBJECT_NEW = 11
            ,
            [EnumDisplayString("Form admin getall object active")]
            FORM_ADMIN_GET_OBJECT_ACTIVE = 12
            ,
            [EnumDisplayString("Form admin getall object refuse")]
            FORM_ADMIN_OBJECT_REFUSE = 13
            ,
            [EnumDisplayString("Form admin getall object disable")]
            FORM_ADMIN_OBJECT_DISABLE = 14
            ,
            [EnumDisplayString("Form admin getall object other")]
            FORM_ADMIN_OBJECT_OTHER = 15
            ,
            [EnumDisplayString("Form partner getall object ")]
            FORM_PARTNER_OBJECT_GETALL = 2
            ,
            [EnumDisplayString("Form partner getall object new  ")]
            FORM_PARTNER_OBJECT_NEW = 21
            ,
            [EnumDisplayString("Form partner get a object ")]
            FORM_PARTNER_A_OBJECT = 22
            ,
            [EnumDisplayString("Form partner get a object ")]
            FORM_PARTNER_OBJECT_DETAIL = 23
            ,
            [EnumDisplayString("Form partner get object other")]
            FORM_PARTNER_OBJECT_OTHER = 24
            ,
            [EnumDisplayString("Form user getall object active")]
            FORM_USER_OBJECT_GETALL = 3
            ,
            [EnumDisplayString("Form user getall object hight rate")]
            FORM_USER_OBJECT_HIGHT_RATE = 31
            ,
            [EnumDisplayString("Form user getall object low rate")]
            FORM_USER_OBJECT_LOW_RATE = 32
            ,
            [EnumDisplayString("Form user get detail objcet")]
            FORM_USER_OBJECT_DETAIL = 33
            ,
            [EnumDisplayString("Form user get object location map")]
            FORM_USER_OBJECT_LOCATIONMAP = 34
            ,

        }

    }


    public static partial class CommonENumItem
    {
        public enum STATE_ITEM
        {
            NEW = 1,
            ACTIVE = 2,
            REFUSE = 3,
            DISABLE = 4

        }
        public enum FORMCASE_GET_DATA
        {
            [EnumDisplayString("Form get all item data")]
            GETALL_ITEM = 1
            ,
            [EnumDisplayString("Form get detail item data")]
            GET_DETAIL_ITEM = 2
            ,

        }

        public enum FORM_ID_ITEM
        {
            [EnumDisplayString("Form admin getall item")]
            FORM_ADMIN_GET_ITEM_GETALL = 1
            ,
            [EnumDisplayString("Form admin getall item new")]
            FORM_ADMIN_GET_ITEM_NEW = 11
            ,
            [EnumDisplayString("Form admin getall item active")]
            FORM_ADMIN_GET_ITEM_ACTIVE = 12
            ,
            [EnumDisplayString("Form admin getall item refuse")]
            FORM_ADMIN_ITEM_REFUSE = 13
            ,
            [EnumDisplayString("Form admin getall item disable")]
            FORM_ADMIN_ITEM_DISABLE = 14
            ,
            [EnumDisplayString("Form admin getall item other")]
            FORM_ADMIN_ITEM_OTHER = 15
            ,
            [EnumDisplayString("Form partner getall item ")]
            FORM_PARTNER_ITEM_GETALL = 2
            ,
            [EnumDisplayString("Form partner getall item new  ")]
            FORM_PARTNER_ITEM_NEW = 21
            ,
            [EnumDisplayString("Form partner get a item ")]
            FORM_PARTNER_ITEM_ACTIVE = 22
            ,
            [EnumDisplayString("Form partner get a item ")]
            FORM_PARTNER_ITEM_DETAIL = 23
            ,
            [EnumDisplayString("Form partner get item other")]
            FORM_PARTNER_ITEM_OTHER = 24
            ,
            [EnumDisplayString("Form partner get item refuse")]
            FORM_PARTNER_ITEM_REFUSE = 25
            ,
            [EnumDisplayString("Form partner get item disable")]
            FORM_PARTNER_ITEM_DISABLE = 26
            ,
            [EnumDisplayString("Form user getall item active")]
            FORM_USER_ITEM_GETALL = 3
            ,
            [EnumDisplayString("Form user getall item hight rate")]
            FORM_USER_ITEM_HIGHT_RATE = 31
            ,
            [EnumDisplayString("Form user getall item low rate")]
            FORM_USER_ITEM_LOW_RATE = 32
            ,
            [EnumDisplayString("Form user get detail item")]
            FORM_USER_ITEM_DETAIL = 33
            ,
            [EnumDisplayString("Form user get all item by object")]
            FORM_USER_ITEM_GETALL_BY_OBJECT = 34
            ,
         

        }

    }
    public static partial class CommonENumRate
    {
       
      
        public enum FORM_ID_RATE
        {
            [EnumDisplayString("Form admin getall review")]
            FORM_ADMIN_GETALL_REVIEW = 1
            ,
            [EnumDisplayString("Form partner getall review")]
            FORM_PARTNER_GETALL_REVIEW = 2
            ,
            [EnumDisplayString("Form user getall review by product")]
            FORM_USER_GETALL_REVIEW_BY_PRODUCT = 3
            ,
            [EnumDisplayString("Form user getall review by shop")]
            FORM_USER_GETALL_REVIEW_BY_SHOP = 31

        }

    }

    public class EnumDisplayString : Attribute
    {
        public string DisplayString;

        public EnumDisplayString(string text)
        {
            this.DisplayString = text;
        }
    }
}
