/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

package org.apache.guacamole.auth;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.PrintWriter;
import java.util.Map;
import java.util.HashMap;
import java.text.SimpleDateFormat;  
import java.util.Date;
import java.text.ParseException;
import org.apache.guacamole.GuacamoleException;
import org.apache.guacamole.net.auth.simple.SimpleAuthenticationProvider;
import org.apache.guacamole.net.auth.Credentials;
import org.apache.guacamole.protocol.GuacamoleConfiguration;
import org.apache.guacamole.environment.Environment;
import org.apache.guacamole.environment.LocalEnvironment;
import org.apache.guacamole.properties.StringGuacamoleProperty;

import org.bouncycastle.crypto.engines.RijndaelEngine;
import org.bouncycastle.crypto.modes.CBCBlockCipher;
import org.bouncycastle.crypto.paddings.PaddedBufferedBlockCipher;
import org.bouncycastle.crypto.paddings.PKCS7Padding;
import org.bouncycastle.crypto.params.KeyParameter;
import org.bouncycastle.crypto.params.ParametersWithIV;
import org.bouncycastle.crypto.CipherParameters;
import org.bouncycastle.util.encoders.Base64;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import org.json.*;

public class SolidCPAuthenticationProvider extends SimpleAuthenticationProvider {
	
	private static final Logger logger = LoggerFactory.getLogger(SolidCPAuthenticationProvider.class);

	final StringGuacamoleProperty solidKey = new StringGuacamoleProperty() {
		@Override
		public String getName() { return "solidcp-key"; }
	};

	final StringGuacamoleProperty linkTime = new StringGuacamoleProperty() {
		@Override
		public String getName() { return "solidcp-link-exp-time"; }
	};

	final StringGuacamoleProperty serverLayout = new StringGuacamoleProperty() {
		@Override
		public String getName() { return "server-layout"; }
	};

	@Override
	public String getIdentifier() {
		return "solidcp-auth";
	}

	@Override
	public Map<String, GuacamoleConfiguration> getAuthorizedConfigurations(Credentials credentials)throws GuacamoleException {

		log("debug", "requesting authentication from " + credentials.getRemoteHostname() + " [" + credentials.getRemoteAddress() + "]");

		Map<String, String[]> paramMap = credentials.getRequest().getParameterMap();
		String eStr;

		try {

			eStr = paramMap.get("e")[0];

		}catch (Exception e) {

			log("debug", "solidcp parameter map does not exist and will not be read");
			return null;

		}

		Environment environment = LocalEnvironment.getInstance();
		String key = environment.getRequiredProperty(solidKey);
		eStr = decrypt(key, eStr);

		if (eStr == null || eStr.isEmpty()) {
			
			log("debug", "invalid parameter map (abort)");
			return null;

		}

		int lTime = 60000;
		try {

			lTime = Integer.parseInt(environment.getRequiredProperty(linkTime))*1000;

		}catch(Exception e) {

			log("warn", "link expiration exception: " + e.toString());
			log("warn", "link expiration failing to default " + String.valueOf(lTime) + "ms");

		}

		String protocol, hypervHost, userName, password, domain, port, security, vmId, vmHostname, timestamp;
		try {

			JSONObject obj = new JSONObject(eStr);
			protocol = obj.getString("protocol");
			hypervHost = obj.getString("hostname");
			userName = obj.getString("username");
			password = obj.getString("password");
			domain = obj.getString("domain");
			port = obj.getString("port");
			security = obj.getString("security");
			vmId = obj.getString("preconnectionblob");
			vmHostname = obj.getString("vmhostname");
			timestamp = obj.getString("timestamp");

		} catch (JSONException e) {

			log("error", "JSON exception (abort): " + e.toString());
			return null;

		}

		Date linkDate;
		try {

			linkDate = new SimpleDateFormat("yyyy-MM-dd_HH:mm:ss").parse(timestamp);

		}catch(ParseException e) {

			log("error", "timestamp exception (abort): " + e.toString());
			return null;

		}

		Date currDate = new Date();
		Long currTime = currDate.getTime();
		Long linkTime = linkDate.getTime();

		log("debug", "current time: " + currTime.toString() + ", stamped time: " + linkTime + ", link exp: " + lTime + "ms");

		if (currTime-linkTime > lTime) {

			log("warn", "link expired (abort)");
			return null;

		}

		Map<String, GuacamoleConfiguration> configs = new HashMap<String, GuacamoleConfiguration>();
		GuacamoleConfiguration config = new GuacamoleConfiguration();
		config.setProtocol(protocol);
		config.setParameter("hostname", hypervHost);
		config.setParameter("port", port);
		config.setParameter("username", userName);
		config.setParameter("password", password);
		config.setParameter("domain", domain);
		config.setParameter("security", security);
		config.setParameter("ignore-cert", "true");
		config.setParameter("disable-auth", "false");
		config.setParameter("preconnection-id", "");
		config.setParameter("preconnection-blob", vmId);

		String layout = environment.getProperty(serverLayout);
		if (layout != null && !layout.isEmpty()) config.setParameter("server-layout", layout);
		configs.put(vmHostname, config);

		if (logger.isDebugEnabled()) {
			log("debug", "authorized connection from " + domain + "/" + userName + " @ " + credentials.getRemoteHostname() + " [" + credentials.getRemoteAddress() + "] to " + vmHostname + " [" + vmId + "] on " + hypervHost + ":" + port + " using " + security);
		}
		else {
			log("info", "authorized connection from " + credentials.getRemoteHostname() + " [" + credentials.getRemoteAddress() + "] to " + vmHostname + " located at " + hypervHost + ":" + port + " using " + security);
		}
		
		return configs;

	}

	private void log(String logType, String msg) {
		try {
			if (logType == "info" && logger.isInfoEnabled()) { logger.info(msg); }
			else if (logType == "warn" && logger.isWarnEnabled()) { logger.warn(msg); }
			else if (logType == "error" && logger.isErrorEnabled()) { logger.error(msg); }
			else if (logType == "debug" && logger.isDebugEnabled()) { logger.debug(msg); }
			else if (logType == "trace" && logger.isTraceEnabled()) { logger.trace(msg); }
		}catch (Exception e) {}
	}

	private String decrypt(String key, String encrypted)
	{
		String[] split = key.split(":");
		if (split.length != 2) return "";
		return decryptWithAesCBC(split[0], split[1], encrypted);
	}

	private String decryptWithAesCBC(String key, String iv, String encrypted)
	{
		try {
			encrypted = new String(Base64.decode(encrypted));
			byte[] bEncrypted = Base64.decode(encrypted);
			byte[] bKey = Base64.decode(key);
			byte[] bIv = Base64.decode(iv);
			PaddedBufferedBlockCipher aes = new PaddedBufferedBlockCipher(new CBCBlockCipher(new RijndaelEngine(256)), new PKCS7Padding());
			CipherParameters ivAndKey = new ParametersWithIV(new KeyParameter(bKey), bIv);
			aes.init(false, ivAndKey);
			return new String(cipherData(aes, bEncrypted));
		} catch (Exception e) {
			log("error", "decryptWithAesCBC exception: " + e.toString());
			return "";
		}
	}

	private byte[] cipherData(PaddedBufferedBlockCipher cipher, byte[] data)
	{
		try {
			int minSize = cipher.getOutputSize(data.length);
			byte[] outBuf = new byte[minSize];
			int length1 = cipher.processBytes(data, 0, data.length, outBuf, 0);
			int length2 = cipher.doFinal(outBuf, length1);
			int actualLength = length1 + length2;
			byte[] cipherArray = new byte[actualLength];
			for (int x = 0; x < actualLength; x++) {
				cipherArray[x] = outBuf[x];
			}
			return cipherArray;
		} catch (Exception e) {
			log("error", "cipherData exception: " + e.toString());
			return null;
		}
	}
}