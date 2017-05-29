CodeMirror.defineMode('htaccess', function() {

  var words = {};
  function define(style, string) {
    var split = string.split(' ');
    for(var i = 0; i < split.length; i++) {
      words[split[i]] = style;
    }
  };

  // Atoms
  define('atom', 'On Off yes no');

  // Keywords
  //define('keyword', 'Files Location');

  // Commands
  define('builtin', 'AddInputFilter AddLanguage AddModuleInfo AddOutputFilter AddOutputFilterbyType AddType Alias AliasMatch Allow AllowConnect AllowEncodedslashes AllowOverride Anonymous Anonymous_LogEmail Anonymous_MustGiveEmail Anonymous_NoUserid Anonymous_VerifyEmail AuthBasicAuthoritative AuthBasicProvider AuthDbdUserpwQuery AuthDbdUserRealmQuery AuthDbmGroupFile AuthDbmType AuthDbmUserFile AuthDefaultAuthoritative AuthDigestAlgorithm AuthDigestDomain AuthDigestProvider AuthGroupFile AuthName AuthType AuthUserFile AuthzDbmAuthoritative AuthzDbmType AuthzDefaultAuthoritative AuthzGroupFileAuthoritative AuthzOwnerAuthoritative AuthzUserAuthoritative BalancerMember BrowserMatch BrowserMatchnocase CacheDebugHeader CacheDefaultExpire CacheDirLength CacheDirLevels CacheDisable CacheEnable CacheFile CacheignoreCachecontrol CacheignoreHeaders CacheignoreNoLastMod CacheignoreQueryString CacheLastModifiedFactor CacheMaxExpire CacheMaxFileSize CacheMinFileSize CacheRoot CacheStoreNoStore CacheStorePrivate CacheVaryByHeaders CacheVaryByParams CheckCaseOnly CheckSpelling CheckBaseName CookieDomain CookieExpires CookieLog CookieName Cookiestyle Cookietracking CustomLog DbdKeep DbdMax DbdMin DbdParams DbdPersist DbdPrepareSql DbDriver Deny DirectoryIndex DirectoryMatch DirectorySlash DOSHashTableSize DOSWhiteList DOSPageCount DOSsiteCount DOSPageInterval DOSsiteInterval DOSBlockingPeriod DOSCloseSocket DOSSystemCommand ErrorDocument ErrorLog ExpiresActive ExpiresByType ExpiresDefault FilterChain FilterDeclare FilterProtocol FilterProvider FreeSites GracefulShutdownTimeout Group Header HeaderName HostNameLookups Include KeepAlive keepAliveTimeout Listen ListenBackLog LoadFile LoadModule LockFile LogFormat LogLevel MCacheMaxObjectCount MCacheMaxObjectSize MCacheMaxStreaMingBuffer MCacheMinObjectSize MCacheRemovalAlgorithm MCacheSize mod_gzip_on  mod_gzip_add_header_count  mod_gzip_compression_level  mod_gzip_keep_workfiles mod_gzip_dechunk mod_gzip_min_http mod_gzip_minimum_file_size mod_gzip_maximum_file_size mod_gzip_maximum_inmem_size mod_gzip_temp_dir mod_gzip_item_include mod_gzip_item_exclude mod_gzip_command_version mod_gzip_can_negotiate mod_gzip_handle_methods mod_gzip_static_suffix mod_gzip_send_vary mod_gzip_update_static NameVirtualHost NoProxy Options Order PassEnv ProxyBadHeader ProxyBlock ProxyDomain ProxyErrorOverride ProxyIoBufferSize ProxyMaxForwards ProxyPass ProxyPassinterpolateEnv ProxyPassMatch ProxyPassReverse ProxyPassReverseCookieDomain ProxyPassReverseCookiepath ProxyPreserveHost ProxyReceiveBufferSize ProxyRemote ProxyRemoteMatch ProxyRequests ProxySet ProxyStatus ProxyTimeout ProxyVia ReceiveBufferSize Redirect RedirectMatch Redirectpermanent Redirecttemp RegistrationName Registrationcode RemoveHandler RemoveInputFilter RemoveLanguage RemoveOutputFilter RemoveType ReplaceFilterDefine ReplacePattern RequestHeader Require RewriteBase RewriteCond RewriteEngine RewriteHeader RewriteLock RewriteLog RewriteLogLevel RewriteMap RewriteOptions RewriteRule RewriteProxy Satisfy SendBufferSize ServerAdmin ServerAlias ServerLimit ServerName ServerPath ServerRoot ServerSignature ServerTokens SetEnv SetEnvif SetEnvifnocase SetHandler SetInputFilter SetOutputFilter SeoRule Substitute Timeout TraceEnable TransferLog TypesConfig UnsetEnv UseCanonicalName UseCanonicalPhysicalPort UserDir VirtualDocumentRoot VirtualDocumentRootip VirtualscriptAlias VirtualscriptAliasIp HotlinkInvolveIp HotlinkExpires HotlinkProtect HotlinkSignature HotlinkType HotlinkError HotlinkAllow HotlinkDeny LinkFreezeEngine LinkFreezePageSizeLimit LinkFreezeRule');

  function tokenBase(stream, state) {

    var sol = stream.sol();
    var ch = stream.next();

    if (ch === '\'' || ch === '"' || ch === '`') {
      state.tokens.unshift(tokenString(ch));
      return tokenize(stream, state);
    }
    if (ch === '#') {
      if (sol && stream.eat('!')) {
        stream.skipToEnd();
        return 'meta'; // 'comment'?
      }
      stream.skipToEnd();
      return 'comment';
    }
    if (ch === '$') {
      stream.eatWhile(/[\w\$_]/);
      return 'def';
    }
    if (ch === '%') {
      stream.eatWhile(/[\w\_{}]/);
      return 'tag';
    }
    if (ch === '+' || ch === '=') {
      return 'operator';
    }
    if (ch === '<') {
      stream.eat('/');
      stream.eatWhile(/[^>]/);
      return 'attribute';
    }
    if (ch === '>') { 
     return 'attribute';
    }

    /*
    if (ch === '-') {
      stream.eat('-');
      stream.eatWhile(/\w/);
      return 'attribute';
    }
    */
    if (/\d/.test(ch)) {
      stream.eatWhile(/\d/);
      if(!/\w/.test(stream.peek())) {
        return 'number';
      }
    }
    stream.eatWhile(/\w/);
    var cur = stream.current();
    return words.hasOwnProperty(cur) ? words[cur] : null;
  }

  function tokenString(quote) {
    return function(stream, state) {
      var next, end = false, escaped = false;
      while ((next = stream.next()) != null) {
        if (next === quote && !escaped) {
          end = true;
          break;
        }
        if (next === '$' && !escaped && quote !== '\'') {
          escaped = true;
          stream.backUp(1);
          state.tokens.unshift(tokenDollar);
          break;
        }
        escaped = !escaped && next === '\\';
      }
      if (end || !escaped) {
        state.tokens.shift();
      }
      return (quote === '`' || quote === ')' ? 'quote' : 'string');
    };
  };

  function tokenize(stream, state) {
    return (state.tokens[0] || tokenBase) (stream, state);
  };

  return {
    startState: function() {return {tokens:[]};},
    token: function(stream, state) {
      if (stream.eatSpace()) return null;
      return tokenize(stream, state);
    }
  };
});
  
CodeMirror.defineMIME('text/x-htaccess', 'htaccess');
CodeMirror.defineMIME('text/x-apache-conf', 'htaccess');

(function() {
  CodeMirror.htaccessHint = function(editor, getHints, givenOptions) {
    // Determine effective options based on given values and defaults.
    var options = {}, defaults = CodeMirror.htaccessHint.defaults;
    for (var opt in defaults)
      if (defaults.hasOwnProperty(opt))
        options[opt] = (givenOptions && givenOptions.hasOwnProperty(opt) ? givenOptions : defaults)[opt];
    
    function collectHints(previousToken) {
      // We want a single cursor position.
      if (editor.somethingSelected()) return;

      var tempToken = editor.getTokenAt(editor.getCursor());

      // Don't show completions if token has changed and the option is set.
      if (options.closeOnTokenChange && previousToken != null &&
          (tempToken.start != previousToken.start || tempToken.type != previousToken.type)) {
        return;
      }

      //var result = getHints(editor, givenOptions);
      var result = {
        list: ['RewriteRule', 'RewriteEngine']
      };
      if (!result || !result.list.length) return;
      var completions = result.list;
      function insert(str) {
        editor.replaceRange(str, result.from, result.to);
      }
      // When there is only one completion, use it directly.
      if (options.completeSingle && completions.length == 1) {
        insert(completions[0]);
        return true;
      }

      // Build the select widget
      var complete = document.createElement("div");
      complete.className = "CodeMirror-completions";
      var sel = complete.appendChild(document.createElement("select"));
      // Opera doesn't move the selection when pressing up/down in a
      // multi-select, but it does properly support the size property on
      // single-selects, so no multi-select is necessary.
      if (!window.opera) sel.multiple = true;
      for (var i = 0; i < completions.length; ++i) {
        var opt = sel.appendChild(document.createElement("option"));
        opt.appendChild(document.createTextNode(completions[i]));
      }
      sel.firstChild.selected = true;
      sel.size = Math.min(10, completions.length);
      var pos = editor.cursorCoords(options.alignWithWord ? result.from : null);
      complete.style.left = pos.left + "px";
      complete.style.top = pos.bottom + "px";
      document.body.appendChild(complete);
      // If we're at the edge of the screen, then we want the menu to appear on the left of the cursor.
      var winW = window.innerWidth || Math.max(document.body.offsetWidth, document.documentElement.offsetWidth);
      if(winW - pos.left < sel.clientWidth)
        complete.style.left = (pos.left - sel.clientWidth) + "px";
      // Hack to hide the scrollbar.
      if (completions.length <= 10)
        complete.style.width = (sel.clientWidth - 1) + "px";

      var done = false;
      function close() {
        if (done) return;
        done = true;
        complete.parentNode.removeChild(complete);
      }
      function pick() {
        insert(completions[sel.selectedIndex]);
        close();
        setTimeout(function(){editor.focus();}, 50);
      }
      CodeMirror.on(sel, "blur", close);
      CodeMirror.on(sel, "keydown", function(event) {
        var code = event.keyCode;
        // Enter
        if (code == 13) {CodeMirror.e_stop(event); pick();}
        // Escape
        else if (code == 27) {CodeMirror.e_stop(event); close(); editor.focus();}
        else if (code != 38 && code != 40 && code != 33 && code != 34 && !CodeMirror.isModifierKey(event)) {
          close(); editor.focus();
          // Pass the event to the CodeMirror instance so that it can handle things like backspace properly.
          editor.triggerOnKeyDown(event);
          // Don't show completions if the code is backspace and the option is set.
          if (!options.closeOnBackspace || code != 8) {
            setTimeout(function(){collectHints(tempToken);}, 50);
          }
        }
      });
      CodeMirror.on(sel, "dblclick", pick);

      sel.focus();
      // Opera sometimes ignores focusing a freshly created node
      if (window.opera) setTimeout(function(){if (!done) sel.focus();}, 100);
      return true;
    }
    return collectHints();
  };
  CodeMirror.htaccessHint.defaults = {
    closeOnBackspace: true,
    closeOnTokenChange: false,
    completeSingle: true,
    alignWithWord: true
  };
})();


