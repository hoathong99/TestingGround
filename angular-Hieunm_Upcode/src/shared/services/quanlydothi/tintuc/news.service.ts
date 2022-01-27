import { Injectable, InjectionToken, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { BaseService } from '../../base.service';
import { NewsDto } from '@shared/models/Quanlydothi/tintuc/news';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

@Injectable()
export class NewsService extends BaseService {

    constructor(
        injector: Injector,
    ) {
        super(injector);
    }


    public createNews(data: NewsDto): Observable<any> {
        return this.http.post(this.urlApi + "/api/services/app/News/Create", data).pipe(
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