import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CityComponent } from './city.component';
import { CityRoutingModule } from './city-routing.module';
import { ChartModule } from 'primeng/chart';
import { MainCityComponent } from './main-city/main-city.component';
import { GMapModule } from 'primeng/gmap';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { DialogModule } from 'primeng/dialog';
import { MultiSelectModule } from 'primeng/multiselect';
import { ButtonModule } from 'primeng/button';
import { InputNumberModule } from 'primeng/inputnumber';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { InputTextModule } from 'primeng/inputtext';
import { ToolbarModule } from 'primeng/toolbar';
import { RadioButtonModule } from 'primeng/radiobutton';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { DropdownModule } from 'primeng/dropdown';
import { FileUploadModule } from 'primeng/fileupload';
import { FormsModule } from '@angular/forms';
import { CheckboxModule } from 'primeng/checkbox';
import { TabViewModule } from 'primeng/tabview';
import { NotificationComponent } from './notification/notification.component';
import { BadgeModule } from 'primeng/badge';
import { ProblemSystemComponent } from './problem-system/problem-system.component';
import { DataViewModule } from 'primeng/dataview';
import { CommunityServiceComponent } from './community-service/community-service.component';
import { NewsComponent } from './news/news.component';
import { OperateComponent } from './operate/operate.component';
import {EditorModule} from 'primeng/editor';
import { AddNewsComponent } from './news/add-news/add-news.component';
import { UpdateDeleteNewsComponent } from './news/update-delete-news/update-delete-news.component';
import { NewsTypePipe } from '@shared/pipes/news-type.pipe';

@NgModule({
  imports: [
    CommonModule,
    CityRoutingModule,
    ChartModule,
    GMapModule,
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
    BadgeModule,
    DataViewModule,
    EditorModule,
  ],
  declarations: [
    CityComponent,
    MainCityComponent,
    NotificationComponent,
    ProblemSystemComponent,
    CommunityServiceComponent,
    NewsComponent,
    OperateComponent,
    AddNewsComponent,
    UpdateDeleteNewsComponent,
    NewsTypePipe,
  ]
})
export class CityModule { }
