import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { Nav } from "../layout/nav/nav";
import { AccountService } from '../core/services/account-service';
import { lastValueFrom } from 'rxjs';
import { Home } from "../features/home/home";
import { User } from '../types/user';
// import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [Nav, Home]
})
export class App implements OnInit {

  private accountService = inject(AccountService)
   private http = inject(HttpClient) ;
  
  //protected readonly title = signal('client');
  protected readonly title = 'Dating App';

protected members = signal<User[]>([]);


  //  ngOnInit(): void {
   
  //   var url = 'https://localhost:5001/api/members';
    
  //   this.http.get(url).subscribe({
  //     next: response => this.members.set(response)  ,
  //     error: error => console.log(error),
  //     complete: () => console.log("Completed http request")
  //   });

  // }

 async ngOnInit() {

  this.members.set(await this.getMembers());
  // console.log(this.members());
  this.setCurrentUser();
}

setCurrentUser()
{
const userString = localStorage.getItem('user');
if ( !userString) return;

const user = JSON.parse(userString);
this.accountService.currentUser.set(user);
}

async getMembers() {

 const url = 'https://localhost:5001/api/members';

  try {
     return lastValueFrom(this.http.get<User[]>(url));
  } 
  catch (error)
   {
    console.log(error);
    throw error;
  }
  
  
}

}