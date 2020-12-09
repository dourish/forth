
variable short
variable long
variable mid

100 short !
500 mid !
10000 long !

: shortpause short @ 1 do loop ;
: longpause long @ 1 do loop ;

255 ddra !

: on 1 porta c! ;
: off 0 porta c! ;

: rippleup 1 porta c! shortpause 2 porta c! shortpause
    4 porta c! shortpause 8 porta c! ;

: rippledown 8 porta c! shortpause 4 porta c! shortpause
    2 porta c! shortpause 1 porta c! ;

: 1blinken rippleup shortpause rippledown shortpause ;

: nblinken 1 do 1blinken loop ;

( pause for @mid counts and exit if a key is pressed )
: ipause mid @ 1 do key? if key drop abort then loop ;

: display begin 1 porta c! ipause 2 porta c! ipause 4 porta c! ipause
    8 porta c! ipause 4 porta c! ipause 2 porta c! ipause 1 porta c!
    ipause repeat ;

: leds porta c! ;

: ripple begin 1 leds ipause 2 leds ipause 4 leds ipause 8 leds ipause
  4 leds ipause 2 leds ipause repeat ;

: setpause leds ipause ;

: pulse begin 1 setpause 3 setpause 7 setpause 15 setpause
              14 setpause 12 setpause 8 setpause
              12 setpause 14 setpause 15 setpause
              7 setpause 3 setpause repeat ;
