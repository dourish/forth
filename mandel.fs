
20 constant maxiter
-39 constant minval
40 constant maxval
20 5 lshift constant rescale
rescale 4 * constant s_escape
variable creal
variable cimag
variable zreal
variable zimag
variable count

: zr_sq zreal @ dup rescale */ ;
: zi_sq zimag @ dup rescale */ ;

: .char dup 18 > if 32 emit drop else
    s" ..,'~!^:;[/<&?oxox#  "
    drop + 1 type then ;

: escapes? s_escape > ;

: count_and_test?
    count @ 1+ dup count !
    maxiter > ;

: init_vars
    5 lshift dup creal ! zreal !
    5 lshift dup cimag ! zimag !
    1 count ! ;


: doescape
    zr_sq zi_sq 2dup +
    escapes? if 2drop true
    else
      - creal @ +
      zreal @ zimag @ rescale */ 1 lshift
     cimag @ + zimag ! zreal !
     count_and_test? then ;

: docell
    init_vars
    begin doescape until count @ .char ;

: dorow maxval minval do dup i docell loop
    drop ;

: mandelbrot
    cr
    maxval minval do
    i dorow cr
    loop ;



