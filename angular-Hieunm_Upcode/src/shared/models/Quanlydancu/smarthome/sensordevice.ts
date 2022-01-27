export interface SensorDeviceDto {
    id?: number;
    name?: string;
    type?: number;
    deviceIp?: string;
    url?: string;
    port?: number;
    homeServerId?: number;
    smartHomeId?: number;
    roomId?: number;
    floorId?: number;
    homeDeviceId?: string;
    parameters?: string;
}