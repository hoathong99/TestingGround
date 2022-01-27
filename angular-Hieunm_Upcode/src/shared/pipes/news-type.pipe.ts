import { Pipe, PipeTransform } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';

@Pipe({
  name: 'newsType'
})
export class NewsTypePipe implements PipeTransform {

  transform(value: number, ...args: unknown[]): unknown {
    return AppConsts.newsType.find(item => item.id == value).name;
  }

}
