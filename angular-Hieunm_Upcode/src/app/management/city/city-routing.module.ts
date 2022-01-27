import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CityComponent } from './city.component';
import { MainCityComponent } from './main-city/main-city.component';
import { ProblemSystemComponent } from './problem-system/problem-system.component';
import { NotificationComponent } from './notification/notification.component';
import { CommunityServiceComponent } from './community-service/community-service.component';
import { NewsComponent } from './news/news.component';
import { OperateComponent } from './operate/operate.component';
import { AddNewsComponent } from './news/add-news/add-news.component';
import { UpdateDeleteNewsComponent } from './news/update-delete-news/update-delete-news.component';

const routes: Routes = [
    {
        path: '',
        component: CityComponent,
        children: [
            {
                path: '', component: MainCityComponent
            },
            { path: 'maincity', component: MainCityComponent },
            {
                path: 'problem', component: ProblemSystemComponent,
            },
            {
                path: 'notification', component: NotificationComponent,
            },
            {
                path: 'service', component: CommunityServiceComponent,
            },
            {
                path: 'news', component: NewsComponent,
            },
            {
                path: 'operate', component: OperateComponent,
            },
            {
                path: 'add-news', component: AddNewsComponent,
            },
            {
                path: 'update-delete-news', component: UpdateDeleteNewsComponent,
            },

        ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class CityRoutingModule { }
