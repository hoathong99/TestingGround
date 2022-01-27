import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommunityComponent } from './community.component';
import { CommunityRoutingModule } from './community-routing.module';
import { RouterModule } from '@angular/router';
import { MainCommunityComponent } from './main-community/main-community.component';
import { SmarthomeSettingComponent } from './smarthome-setting/smarthome-setting.component';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { CalendarModule } from 'primeng/calendar';
import { SliderModule } from 'primeng/slider';
import { MultiSelectModule } from 'primeng/multiselect';
import { ContextMenuModule } from 'primeng/contextmenu';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { ProgressBarModule } from 'primeng/progressbar';
import { InputTextModule } from 'primeng/inputtext';
import { FileUploadModule } from 'primeng/fileupload';
import { ToolbarModule } from 'primeng/toolbar';
import { RatingModule } from 'primeng/rating';
import { RadioButtonModule } from 'primeng/radiobutton';
import { InputNumberModule } from 'primeng/inputnumber';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { MessageService } from 'primeng/api';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { FormsModule } from '@angular/forms';
import { CheckboxModule } from 'primeng/checkbox';
import { EditSmarthomeComponent } from './smarthome-setting/edit-smarthome/edit-smarthome.component';
import { DeviceSmarthomeComponent } from './device-smarthome/device-smarthome.component';
import { ConnectApiComponent } from './connect-api/connect-api.component';
import { TabViewModule } from 'primeng/tabview';
import { ChartModule } from 'primeng/chart';


@NgModule({
  imports: [
    CommonModule,
    CommunityRoutingModule,
    RouterModule,
    TableModule,
    ToastModule,
    DialogModule,
    MultiSelectModule,
    ButtonModule,
    InputNumberModule,
    ConfirmDialogModule,
    InputTextModule,
    ToolbarModule,
    RadioButtonModule,
    InputTextareaModule,
    DropdownModule,
    FileUploadModule,
    FormsModule,
    CheckboxModule,
    TabViewModule,
    ChartModule
  ],
  declarations: [
    CommunityComponent,
    MainCommunityComponent,
    SmarthomeSettingComponent,
    EditSmarthomeComponent,
    DeviceSmarthomeComponent,
    ConnectApiComponent
  ],
  providers: [MessageService, ConfirmationService]
})
export class CommunityModule { }
