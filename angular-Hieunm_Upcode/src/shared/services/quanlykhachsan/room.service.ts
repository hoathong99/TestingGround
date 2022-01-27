import { InjectionToken, Injectable, Injector } from "@angular/core";
import { Observable } from "rxjs";
import { BaseService } from "../base.service";
import { catchError, map } from 'rxjs/operators';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class RoomService extends BaseService {
    constructor(
        injector: Injector,
    ) {
        super(injector);
    }

    public GetRoomByHotelId(hotelId:number) : Observable<any> {
        return this.http.get(this.urlApi + "/api/services/app/RoomHotel/GetRoomsByHotelId?hotelId="+hotelId).pipe(
            map((res: any) => {
                return res.result.data;
            }),
            catchError(err => {
                console.log("error ", err);
                return null;
            })
        );
    }
}