import { Injectable, InjectionToken, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { HotelDto} from '@shared/models/Quanlykhachsan/hotel';
import { catchError, map } from 'rxjs/operators';
import { BaseService } from '../base.service';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class HotelService extends BaseService {


    constructor(
        injector: Injector,
    ) {
        super(injector);
    }


    public createHotel(data: HotelDto): Observable<any> {
        return this.http.post(this.urlApi + "/api/services/app/Hotel/CreateHotel", data).pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }
    public updateHotel(data: HotelDto): Observable<any> {
        return this.http.put(this.urlApi + "/api/services/app/Hotel/UpdateHotel", data).pipe(
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