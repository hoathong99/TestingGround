import { DeviceDto } from "./device";


export interface SmartHomeSetting {
    smarthome: SmartHomeDto;
    floors: FloorDto;
}
export interface SmartHomeDto {
    id?: number;
    name?: string;
    userName?: string;
    smartHomeCode?: string;
    address?: string;
    scope?: string;
    userId?: number;
    refreshToken?: string;
    numberRooms?: number;
    numberFloors?: number;
    numberDevices?: number;
    numberHomeServers?: number;
    note?: string;
    creationTime?: Date;
    lastModificationTime?: Date;
    homeServerId?: number;
}


export interface IRoomDto {
    id?: number;
    name?: string;
    number?: string;
    imageUrl?: string;
    homeServerId?: number;
    smartHomeId?: number;
    floorId?: number;
    numberDevices?: number;
    devices?: DeviceDto[];
    isSmartLighting?: boolean;
    isSmartCurtain?: boolean;
    isSmartAir?: boolean;
    isSmartWatter?: boolean;
    isSmartDoorEntry?: boolean;
    isSmartConnection?: boolean;
    isSmartConditioner?: boolean;
    isSmartFireAlarm?: boolean;
    isSmartSecurity?: boolean;

    lightingId?: number;
    curtainId?: number;
    airId?: number;
    watterId?: number;
    doorEntryId?: number;
    connectionId?: number;
    conditionerId?: number;
    fireAlarmId?: number;
    securityId?: number;

    lightingNumber?: number;
    curtainNumber?: number;
    airNumber?: number;
    watterNumber?: number;
    doorEntryNumber?: number;
    connectionNumber?: number;
    conditionerNumber?: number;
    fireAlarmNumber?: number;
    securityNumber?: number;

}
export class RoomDto implements IRoomDto {
    id: number | undefined;
    name: string | undefined;
    number: string | undefined;
    floorId: number | undefined;
    smartHomeId: number | undefined;
    numberRooms: number | undefined;
    imageUrl: string | undefined;
    numberDevices: number | undefined;
    devices?: DeviceDto[] | undefined;

    isSmartLighting?: boolean;
    isSmartCurtain?: boolean;
    isSmartAir?: boolean;
    isSmartWatter?: boolean;
    isSmartDoorEntry?: boolean;
    isSmartConnection?: boolean;
    isSmartConditioner?: boolean;
    isSmartFireAlarm?: boolean;
    isSmartSecurity?: boolean;

    lightingId?: number;
    curtainId?: number;
    airId?: number;
    watterId?: number;
    doorEntryId?: number;
    connectionId?: number;
    conditionerId?: number;
    fireAlarmId?: number;
    securityId?: number;

    lightingNumber?: number;
    curtainNumber?: number;
    airNumber?: number;
    watterNumber?: number;
    doorEntryNumber?: number;
    connectionNumber?: number;
    conditionerNumber?: number;
    fireAlarmNumber?: number;
    securityNumber?: number;

    constructor(data?: IRoomDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
        this.lightingNumber = 0;
        this.curtainNumber = 0;
        this.airNumber = 0;
        this.watterNumber = 0;
        this.doorEntryNumber = 0;
        this.connectionNumber = 0;
        this.conditionerNumber = 0;
        this.fireAlarmNumber = 0;
        this.securityNumber = 0;
    }

    init(data?: any) {
        if (data) {
            this.id = data["id"];
            this.name = data["name"];
            this.number = data["number"];
            this.smartHomeId = data["smartHomeId"];
            this.numberRooms = data["numberRooms"];
            this.imageUrl = data["imageUrl"];
            this.numberDevices = data["numberDevices"];
            this.floorId = data["floorId"];

        }
    }

}


export interface IFloorDto {
    id?: number;
    name?: string;
    number?: string;
    smartHomeId?: number;
    numberRooms?: number;
    imageUrl?: string;
    numberDevices?: number;
    rooms?: RoomDto[];
}

export class FloorDto implements IFloorDto {
    id: number | undefined;
    name: string | undefined;
    number: string | undefined;
    smartHomeId: number | undefined;
    numberRooms: number | undefined;
    imageUrl: string | undefined;
    numberDevices: number | undefined;
    rooms: RoomDto[] | undefined;

    constructor(data?: IFloorDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.id = data["id"];
            this.name = data["name"];
            this.number = data["number"];
            this.smartHomeId = data["smartHomeId"];
            this.numberRooms = data["numberRooms"];
            this.imageUrl = data["imageUrl"];
            this.numberDevices = data["numberDevices"];
            if (Array.isArray(data["rooms"])) {
                this.rooms = [] as any;
                for (let item of data["rooms"])
                    this.rooms.push(item);
            }
        }
    }

}


export interface HouseDto {
    id?: number;
    name?: string;
    smartHomeCode?: string;
    address?: string;
}