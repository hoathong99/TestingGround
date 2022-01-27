import { Injectable, InjectionToken, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { BaseService } from '../base.service';
import { GuestDto } from '@shared/models/Quanlykhachsan/guest';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class GuestService extends BaseService {

    constructor(
        injector: Injector,
    ) {
        super(injector);
    }


    public createGuestHotel(data: GuestDto): Observable<any> {
        return this.http.post(this.urlApi + "/api/services/app/GuestHotel/Create", data).pipe(
            map((res: any) => {
                return res;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }

    public getGuestByHotelId(hotelId:number): Observable<any> {
        return this.http.get(this.urlApi + "/api/services/app/GuestHotel/GetGuestsByHotelId?hotelId="+hotelId ).pipe(
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