const BASE_URL = 'http://localhost:53031/api/';

export default class HttpService {
    constructor($http) {
        'ngInject';
        this.http = $http;
    }

    get(additionalUrl) {
		  console.log(additionalUrl);
        return this.ajax('get', additionalUrl);
    }

    post(additionalUrl, entity) {
        return this.ajax('post', additionalUrl, entity);
    }

    put(additionalUrl, entity) {
        return this.ajax('put', additionalUrl, entity);
    }
	 
	 remove(additionalUrl, entity){
		 this.ajax('delete', additionalUrl, entity);
	 }

    ajax(method, additionalUrl, entity) {
        var options = {
            method: method,
            url: BASE_URL + additionalUrl,
            headers: {
                'Content-Type': 'application/json'
            }
        };
        if (entity) {
            options.data = entity;

        }
        return this.http(options).then(successCallback, errorCallback);
    }
}

function successCallback(response) {
    console.log(response.status);
    return response.data;
}

function errorCallback(response) {
    console.log(response.status);
}