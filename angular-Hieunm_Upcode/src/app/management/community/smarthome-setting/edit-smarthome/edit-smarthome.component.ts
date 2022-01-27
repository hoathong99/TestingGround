import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { FloorDto, RoomDto, SmartHomeDto, SmartHomeSetting } from '@shared/models/Quanlydancu/smarthome/smarthome';
import { DeviceService } from '@shared/services/quanlydancu/device.service';
import { HomeserverService } from '@shared/services/quanlydancu/homeserver.service';
import { SmarthomeService } from '@shared/services/quanlydancu/smarthome.service';
import { SelectItem } from 'primeng/api';

interface DV {
  name: string,
  code: string
}

@Component({
  selector: 'app-edit-smarthome',
  templateUrl: './edit-smarthome.component.html',
  styleUrls: ['./edit-smarthome.component.css'],
  animations: [appModuleAnimation()],
})
export class EditSmarthomeComponent extends AppComponentBase implements OnInit {

  smarthomeId: number;
  smarthomeCode: string;

  smarthomeSetting: SmartHomeSetting;

  editSmartHome: SmartHomeDto;
  editRoom: RoomDto;
  floorSH: FloorDto;
  columns: FloorDto[] = [];

  deviceoptions: SelectItem[];
  homeserveroption: SelectItem[];
  lightoptions: SelectItem[];

  floor: FloorDto;
  floors: FloorDto[] = [];

  floorid: number;
  roomid: number;
  dialogEditRoom: boolean = false;

  lighting: boolean = false;
  curtain: boolean = false;
  air: boolean = false;
  security: boolean = false;
  door: boolean = false;
  connection: boolean = false;
  water: boolean = false;
  conditioner: boolean = false;

  constructor(
    injector: Injector,
    private router: Router,
    private smarthomeService: SmarthomeService,
    private route: ActivatedRoute,
    private deviceService: DeviceService,
    private homeserverService: HomeserverService
  ) {
    super(injector);
    this.smarthomeId = Number(this.route.snapshot.paramMap.get('id'));
    console.log('a', this.route.snapshot.paramMap.get('id'));
  }

  ngOnInit() {
    this.loadData();
    var floor = new FloorDto();
    var room = new RoomDto();
    room.name = 'Phòng 1';
    floor.rooms = [room];
    floor.name = 'Tầng 1';
    this.columns = [floor];
  }

  //#region  load data
  public loadData() {
    this.getDetail();
    this.getListDevices();
    this.getListHomeServer();
  }

  getDetail() {
    this.smarthomeService.getById(this.smarthomeId, '/api/services/app/SmartHome/GetByIdSmartHome').subscribe(
      res => {
        if (res.success) {
          this.editSmartHome = res.result.data;
          //this.editSmartHome.lastModificationTime = new Date(this.editSmartHome.lastModificationTime);
          console.log('editSmartHome', this.editSmartHome);
        }
        else {
          this.messageP.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
        }
      }
    )
  }

  getSetting() {
    this.smarthomeService.getByCode(this.smarthomeCode, '/api/services/app/SmartHome/GetSettingSmartHome').subscribe(
      res => {
        if (res.success) {
          this.smarthomeSetting = res.result;
          console.log('getSetting', this.smarthomeSetting);
        }
        else {
          this.messageP.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
        }
      }
    )
  }


  protected getListDevices() {
    const url = "/api/services/app/Device/GetAllDevice";
    return this.deviceService.getAllData(url).subscribe(
      res => {
        if (res.success) {
          this.deviceoptions = res.result.data.map(val => ({
            value: val.id,
            label: val.name
          }));
          this.lightoptions = res.result.data.filter(x => x.typeDevice.includes('Smart Lighting')).map(val => ({
            value: val.id,
            label: val.name
          }));
          console.log('light', this.lightoptions);
          console.log('light', res.result.data);
        }
        else {
          this.messageP.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
        }
      }
    )

  }

  protected getListHomeServer() {
    const url = "/api/services/app/HomeServer/GetAllHomeServer";
    return this.homeserverService.getAllData(url).subscribe(
      res => {
        if (res.success) {
          this.homeserveroption = res.result.data.map(val => ({
            value: val.id,
            label: val.name
          }));
          console.log('homeservers', this.homeserveroption);
        }
        else {
          this.messageP.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
        }
      }
    )

  }


  //#endregion


  //#region  function

  addColumn() {
    var floor = new FloorDto();
    floor.name = 'Tầng ' + (this.columns.length + 1);
    var room = new RoomDto();
    room.name = 'Phòng 1';
    floor.rooms = [room];
    this.columns.push(floor);
  }

  removeColumn() {
    this.columns.splice(-1, 1);
  }

  addRooms(id: number) {
    console.log('this.columns[index]', this.columns[id].rooms);
    this.columns.forEach((x, index) => {

      if (index == id) {
        if (this.columns[index]) {
          var room = new RoomDto();
          room.name = 'Phòng ' + (this.columns[index].rooms.length + 1);
          this.columns[index].rooms.push(room)
        } else {
          this.columns[index].rooms = [];
        }
      }
    });
  }

  minusRooms(id: number) {
    this.columns.forEach((x, index) => {

      if (index == id) {
        if (this.columns[index]) {
          this.columns[index].rooms.splice(-1, 1);
        } else {
          this.columns[index].rooms = [];
        }
      }
    });

  }

  closeEditRoom() {
    this.dialogEditRoom = false;
    var room = new RoomDto();
    room.name = 'Phòng ' + (this.roomid + 1);
    this.columns[this.floorid].rooms[this.roomid] = room;
  }

  submitEditRoom() {
    this.dialogEditRoom = false;
    this.messageP.add({ severity: 'success', summary: '', detail: 'Cấu hình phòng thành công !', life: 4000 });
  }

  openEditRoom(item: RoomDto, p: number, i: number) {
    this.dialogEditRoom = true;
    this.editRoom = item;
    console.log('edit id', item, p, i);
    this.floorid = i;
    this.roomid = p;
  }

  //#endregion

  submitEdit() {
    const url = "/api/services/app/SmartHome/UpdateSmartHome";
    var data = {
      smarthome: this.editSmartHome,
      floors: this.columns
    }
    console.log('data', data);
    return this.smarthomeService.updateSmartHome(data, url).subscribe(
      res => {
        if (res.success) {
          this.messageP.add({ severity: 'success', summary: '', detail: 'Update thành công!', life: 4000 });
        }
        else {
          this.messageP.add({ severity: 'error', summary: '', detail: 'Hệ thống có lỗi !', life: 4000 });
        }
      }
    )

  }
}
