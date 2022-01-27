export class AppConsts {

    static remoteServiceBaseUrl: string;
    static appBaseUrl: string;
    static appBaseHref: string; // returns angular's base-href parameter value if used during the publish

    static localeMappings: any = [];

    static readonly userManagement = {
        defaultAdminUserName: 'admin'
    };

    static readonly localization = {
        defaultLocalizationSourceName: 'MHPQ'
    };

    static readonly authorization = {
        encryptedAuthTokenName: 'enc_auth_token'
    };

    /**
     * Loại tin tức
     * 08/09/21
     */
    static readonly newsType = [
        {name: 'Tin hót', code: '', id:1},
        {name: 'Tin mới', code: '', id:2},
        {name: 'Tin khu dân', code: '',id:3},
        {name: 'Tin thời sự', code: '', id:4},
        {name: 'Tin nước ngoài', code: '', id:5}
      ];
}
