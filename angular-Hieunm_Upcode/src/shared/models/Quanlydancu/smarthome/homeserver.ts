export interface HomeServerDto {
    id?: number;
    name?: string;
    type?: string;
    ip?: string;
    port?: number;
    typeAuth?: string;
    tokenType?: number;
    scope?: string;
    tokenAuth?: string;
    refreshToken?: string;
    homeServerCode?: string;
    producer?: string;
    userLogin?: string;
    password?: string;
}
