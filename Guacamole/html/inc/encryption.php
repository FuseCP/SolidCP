<?php

class encryption {
  function urlsafe_b64decode($string) {
    $data = str_replace(array('-','_'),array('+','/'),$string);
    $mod4 = strlen($data) % 4;
    if ($mod4) {
        $data .= substr('====', $mod4);
    }
    return base64_decode($data);
  }

  function decrypt($key,$encrypted)
  {
    list($ekey, $eiv) = explode(':', $key);
    return $this->decryptRJ256($ekey,$eiv,$encrypted);
  }

  function decryptRJ256($key,$iv,$encrypted)
  {
    //PHP strips "+" and replaces with " ", but we need "+" so add it back in...
    $encrypted = str_replace(' ', '+', $encrypted);
    $iv = str_replace(' ', '+', $iv);
    $key = str_replace(' ', '+', $key);

    //get all the bits
    $key = base64_decode($key);
    $iv = base64_decode($iv);
    $encrypted = base64_decode($encrypted);

    $rtn = mcrypt_decrypt(MCRYPT_RIJNDAEL_256, $key, $encrypted, MCRYPT_MODE_CBC, $iv);
    $rtn = $this->unpad($rtn);
    return($rtn);
  }

  //removes PKCS7 padding
  function unpad($value)
  {
    $blockSize = mcrypt_get_block_size(MCRYPT_RIJNDAEL_256, MCRYPT_MODE_CBC);
    $packing = ord($value[strlen($value) - 1]);
    if($packing && $packing < $blockSize)
    {
        for($P = strlen($value) - 1; $P >= strlen($value) - $packing; $P--)
        {
            if(ord($value{$P}) != $packing)
            {
                $packing = 0;
            }
        }
    }

    return substr($value, 0, strlen($value) - $packing);
  }
}

class randomgen {

  function generateStrongPassword($length = 16, $add_dashes = false, $available_sets = 'lud') {
        $sets = array();
        if(strpos($available_sets, 'l') !== false)
                $sets[] = 'abcdefghjkmnpqrstuvwxyz';
        if(strpos($available_sets, 'u') !== false)
                $sets[] = 'ABCDEFGHJKMNPQRSTUVWXYZ';
        if(strpos($available_sets, 'd') !== false)
                $sets[] = '23456789';
        if(strpos($available_sets, 's') !== false)
                $sets[] = '!@#$%&*?';
        $all = '';
        $password = '';
        foreach($sets as $set)
        {
                $password .= $set[array_rand(str_split($set))];
                $all .= $set;
        }
        $all = str_split($all);
        for($i = 0; $i < $length - count($sets); $i++)
                $password .= $all[array_rand($all)];
        $password = str_shuffle($password);
        if(!$add_dashes)
                return $password;
        $dash_len = floor(sqrt($length));
        $dash_str = '';
        while(strlen($password) > $dash_len)
        {
                $dash_str .= substr($password, 0, $dash_len) . '-';
                $password = substr($password, $dash_len);
        }
        $dash_str .= $password;
        return $dash_str;
  }
}

?>
