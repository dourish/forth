
( print a word as binary )
: %.
  32768
  16 0 do
    2dup and 0= 0= negate
    $30 + emit
    1 rshift
  loop
  drop drop 32 emit ;

( print a byte as binary )
: %c.
  128
  8 0 do
    2dup and 0= 0= negate
    $30 + emit
    1 rshift
  loop
  drop drop 32 emit ;


( print a byte in hex, in a two-character space )
: $c. hex 3 .r decimal ;

( print a word in hex, in a four-character space )
: $. hex 4 .r decimal ;
