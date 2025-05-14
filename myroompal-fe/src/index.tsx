import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import './scss/main.scss'
import 'react-toastify/dist/ReactToastify.css'
import React, {useEffect} from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import reportWebVitals from './reportWebVitals';
import './css/index.css';
import {Auth0Provider, useAuth0} from "@auth0/auth0-react";
import {authInterceptor} from "./helpers/AuthInterceptor";
import {config} from "./config";

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

const onRedirectCallback = (appState: any) => {
    window.history.replaceState(
        {}, 
        document.title, 
        appState?.returnTo || window.location.pathname
    );
};

// Inject the Auth0 user and token getter into the interceptor
function AuthInject() {
    const { getAccessTokenSilently } = useAuth0();

    useEffect(() => {
        authInterceptor.setAuthGetter(getAccessTokenSilently);
        return () => authInterceptor.setAuthGetter(undefined);
    }, [getAccessTokenSilently]);

    return null;
}

root.render(
        <Auth0Provider
            domain={config.authDomain}
            clientId={config.clientId}
            authorizationParams={{
                redirect_uri: window.location.origin,
                audience: config.audience,
                scope: "openid profile email offline_access"
            }}
            cacheLocation="localstorage"
            useRefreshTokens={true}
            onRedirectCallback={onRedirectCallback}
        >
            <React.StrictMode>
                <AuthInject />
                <App />
            </React.StrictMode>
    </Auth0Provider>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
