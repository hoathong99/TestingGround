import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommunityComponent } from './community.component';
import { MainCommunityComponent } from './main-community/main-community.component';
import { SmarthomeSettingComponent } from './smarthome-setting/smarthome-setting.component';
import { EditSmarthomeComponent } from './smarthome-setting/edit-smarthome/edit-smarthome.component';
import { DeviceSmarthomeComponent } from './device-smarthome/device-smarthome.component';
import { ConnectApiComponent } from './connect-api/connect-api.component';


const routes: Routes = [
    {
        path: '',
        component: CommunityComponent,
        children: [
            {
                path: '', component: MainCommunityComponent
            },
            { path: 'maincommunity', component: MainCommunityComponent },
            {
                path: 'smarthomesetting', component: SmarthomeSettingComponent,
            },
            { path: 'editsmarthome/:id', component: EditSmarthomeComponent },

            { path: 'devices', component: DeviceSmarthomeComponent },
            {
                path: 'connectapi', component: ConnectApiComponent
            }

        ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class CommunityRoutingModule { }
