import {HttpClient} from 'aurelia-http-client';
import { inject } from 'aurelia-dependency-injection';
import { observable } from 'aurelia-framework';

@inject(HttpClient)
export class account {
    signedIn= null;
    @observable status= null;

    constructor(httpClient) {
        let self = this;
        httpClient.get('/Accounts/GetStatus')
            .then(data => {
                self.status = JSON.parse(data.response);
                self.signedIn = self.status.isAuthenticated;
            });
    }

    signIn() {
        window.location = "/Accounts/signin";
    }
}