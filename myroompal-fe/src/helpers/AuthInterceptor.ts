import axios, { InternalAxiosRequestConfig } from 'axios';

// Class for intercepting requests and adding the Authorization header
class AuthInterceptor {
    private getToken: any  = undefined;

    // Set the function that will be used to get the token
    setAuthGetter(getToken: any) {
        this.getToken = getToken;
    }

    // Needed for binding the function to the class
    constructor() {
        this.intercept = this.intercept.bind(this);
        this.setAuthGetter = this.setAuthGetter.bind(this);
    }

    // Function that is called before the request is sent
    async intercept(config: InternalAxiosRequestConfig): Promise<InternalAxiosRequestConfig> {
        if (!this.getToken) {
            return config;
        }

        try {
            const token = await this.getToken();
            config.headers = config.headers || {};
            config.headers['Authorization'] = `Bearer ${token}`;
        } catch (e) {
            console.log(e);
        }
        return config;
    }
}

// Create an instance of the class and export it
export const authInterceptor = new AuthInterceptor();
axios.interceptors.request.use(authInterceptor.intercept.bind(authInterceptor));
