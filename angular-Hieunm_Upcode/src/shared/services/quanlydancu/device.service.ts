import { Injectable, InjectionToken, Injector } from '@angular/core';
import { DeviceDto } from '@shared/models/Quanlydancu/smarthome/device';
import { DeviceAPIDto } from '@shared/models/Quanlydancu/smarthome/deviceapi';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { BaseService } from '../base.service';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class DeviceService extends BaseService {

    constructor(
        injector: Injector,
    ) {
        super(injector);
    }

    public createOrUpdateDevice(data: DeviceDto): Observable<any> {
        return this.http.post(this.urlApi + "/api/services/app/Device/CreateOrUpdateDevice", data).pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }


    //#region API device
    public createOrUpdateDeviceApi(data: DeviceAPIDto): Observable<any> {
        return this.http.post(this.urlApi + "/api/services/app/Device/CreateOrUpdateDeviceAPI", data).pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }
    //#endregion
}
