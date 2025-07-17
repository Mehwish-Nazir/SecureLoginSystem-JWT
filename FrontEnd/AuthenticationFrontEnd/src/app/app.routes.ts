import { HttpClientModule } from '@angular/common/http';
import { RouterModule ,RouterOutlet,Routes} from '@angular/router';
import { FormsModule } from '@angular/forms';
import { UserRegistrationComponent } from './Components/User/user-registration/user-registration.component';
import { AppComponent } from './app.component';
import { LoginComponent } from './Components/login/login.component';


export const routes: Routes = [
       //{path:"" },
       {path:'registration' ,  component:UserRegistrationComponent},
       {path:'login', component:LoginComponent}
];
