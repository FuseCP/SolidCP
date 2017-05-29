function CheckAuthenticationExpiration(authTimeOutCookieName, authCookieName, logoutUrl) {
    var c = $.cookie(authTimeOutCookieName);
    
    if (c != null && c != "" && !isNaN(c)) {
        var now = new Date();
        var ms = parseInt(c, 10);
        var expiration = new Date().setTime(ms);
        if (now > expiration) {
            $.removeCookie(authTimeOutCookieName, { path: '/' });
            window.location.replace(logoutUrl);
        }
    }
}

function StartAuthExpirationCheckTimer(authTimeOutCookieName, authCookieName, logoutUrl) {
    setInterval(function() {
        CheckAuthenticationExpiration(authTimeOutCookieName, authCookieName, logoutUrl);
    }, 20000);
}