timeout=3
interval=1
delay=1

sudo -i &

pid=$!

(
    ((t = timeout))

    while ((t > 0)); do
        sleep $interval
        kill -0 $pid || exit 0
        ((t -= interval))
    done

    # Be nice, post SIGTERM first.
    # The 'exit 0' below will be executed if any preceeding command fails.
    kill -s SIGTERM $pid && kill -0 $pid || exit 0
    sleep $delay
    kill -s SIGKILL $pid
) 2> /dev/null &
