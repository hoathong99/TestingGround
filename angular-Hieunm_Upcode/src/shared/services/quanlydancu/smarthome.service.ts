import { Injectable, InjectionToken, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { SmartHomeDto, RoomDto, FloorDto, HouseDto } from '@shared/models/Quanlydancu/smarthome/smarthome';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { DataResponse } from '../../models/data/dataresponse';
import { BaseService } from '../base.service';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class SmarthomeService extends BaseService {


    constructor(
        injector: Injector,
    ) {
        super(injector);
    }


    public createOrUpdateSmartHome(data: SmartHomeDto): Observable<any> {
        return this.http.post(this.urlApi + "/api/services/app/SmartHome/CreateOrUpdateSmartHome", data).pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }

    public updateSmartHome(data: any, url: string): Observable<any> {
        return this.http.put(this.urlApi + url, data).pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }
}
