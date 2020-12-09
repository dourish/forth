8191 constant size
variable flags size allot \ make array with size bytes

: primes (  -- count )
  flags size 1+ 1 fill
  0
  size 0 do
    flags i + c@ if
      i dup + 3 + dup i + 
      begin
      dup size > invert while
          0 over flags + c!
          over +
        repeat
      drop
      . 1+
    then
  loop ." total: " .  ;

: qprimes (  -- count )
  flags size 1+ 1 fill
  0
  size 0 do
    flags i + c@ if
      i dup + 3 + dup i + 
      begin
      dup size > invert while
          0 over flags + c!
          over +
        repeat
      drop drop 1+
    then
  loop ." total: " .  ;

