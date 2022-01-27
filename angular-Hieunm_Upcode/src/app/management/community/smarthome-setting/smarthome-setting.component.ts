import { Component, Injector, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { UserDto, UserDtoPagedResultDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { SmarthomeService } from '@shared/services/quanlydancu/smarthome.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { SmartHomeDto, RoomDto, FloorDto, HouseDto } from '@shared/models/Quanlydancu/smarthome/smarthome';
import { AppComponentBase } from '@shared/app-component-base';
import { DataResponse } from '@shared/models/data/dataresponse';
import { LazyLoadEvent } from 'primeng/api';
import { Observable } from 'rxjs';
import { Table } from 'primeng/table';

export interface Product {
  id?: string;
  code?: string;
  name?: string;
  description?: string;
  price?: number;
  quantity?: number;
  inventoryStatus?: string;
  category?: string;
  image?: string;
  rating?: number;
}
class PagedUsersRequestDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
}
@Component({
  selector: 'app-smarthome-setting',
  templateUrl: './smarthome-setting.component.html',
  styleUrls: ['./smarthome-setting.component.css'],
  animations: [appModuleAnimation()],
})
export class SmarthomeSettingComponent extends AppComponentBase implements OnInit {

  dataSource: Observable<SmartHomeDto[]>;
  smarthomes: SmartHomeDto[] = [];
  rooms: RoomDto[] = [];
  floors: FloorDto[] = [];
  products: Product[];
  dataResponse: DataResponse;
  product: Product;

  selectedSmartHomes: SmartHomeDto[];
  createSmartHome: SmartHomeDto;
  smartHomeRes: SmartHomeDto;
  loading: boolean;
  smartHomeCode: string;
  newsmarthome: boolean = false;
  submitted: boolean = false;
  dialogCode: boolean = false;


  @ViewChild('#dt')
  private tableRef: Table;

  constructor(
    injector: Injector,
    private router: Router,
    private smarthomeService: SmarthomeService
  ) {
    super(injector);

  }


  ngOnInit(): void {
    this.refresh();
    this.loading = true;
    //this.primengConfig.ripple = true;

  }

  refresh(): void {
    this.getDataPage();
  }

  public getDataPage(): void {
    this.getListSmartHome();
  }

  protected getListSmartHome() {
    const url = "/api/services/app/SmartHome/GetAllSmartHome";
    return this.smarthomeService.getAllData(url).subscribe(
      res => {
        this.dataResponse = res.result as any;
        console.log('res', res);
        if (res.success) {
          this.smarthomes = res.result.data;
          this.dataSource = res.result.data;
          this.loading = false;
        }
        else {
          this.messageP.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
        }
      }
    )

  }


  editSmartHome(data: SmartHomeDto) {
    this.router.navigate(['../editsmarthome']);
  }

  addNewSmartHome() {
    this.newsmarthome = true;
    this.createSmartHome = { };
  }

  closeNewSmartHome() {
    this.newsmarthome = false;
    console.log('create', this.createSmartHome);
  }

  submitNewSmartHome() {
    console.log('create', this.createSmartHome);
    this.smarthomeService.createOrUpdateSmartHome(this.createSmartHome).subscribe(
      res => {
        if (res.success) {
          this.smartHomeRes = res.result;
          if (this.smartHomeRes) {
            this.smarthomes.unshift(this.smartHomeRes);
            this.smartHomeCode = this.smartHomeRes.smartHomeCode;
            this.dialogCode = true;
            console.log('code', this.smarthomes);
          }


          this.messageP.add({ severity: 'success', summary: '', detail: 'Tạo mới smarthome thành công!', life: 4000 });
        }
        else {
          this.messageP.add({ severity: 'error', summary: '', detail: 'Tạo smarthome thất bại !', life: 4000 });
        }
      }
    );
    this.newsmarthome = false;
  }


  deleteSmarthome(id: number) {
    const url = "/api/services/app/SmartHome/DeleteSmartHome?id=";
    this.smarthomeService.deleteById(id, url).subscribe(
      res => {
        if (res.success) {
          this.messageP.add({ severity: 'success', summary: '', detail: 'Xóa smarthome thành công!', life: 4000 });
          this.smarthomes = this.smarthomes.filter(obj => obj.id !== id);
        }
        else {
          this.messageP.add({ severity: 'error', summary: '', detail: 'Xóa thất bại !', life: 4000 });
        }
      }
    );
  }


}
