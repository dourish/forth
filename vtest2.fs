( test routines for TMS9918A video card attached to user port )

\ these constants are for the user port
$8010 constant data    \ data on port b
$8011 constant control \ control on port a
$8012 constant ddrb
$8013 constant ddra

\ these constants are the control line assignments for
\ different operations
15 constant inactive  \ mode high, /csw high, /csr high, /reset high
14 constant reset     \ mode high, /csw high, /csr high, /reset low
13 constant regread   \ mode high, /csw high, /csr low, /reset high
11 constant regwrite  \ mode high, /csw low, /csr high, /reset high
7 constant modelow    \ mode low, /csw high, /csr high, /reset high
5 constant vramread   \ mode low, /csw high, /csr low, /reset high 
3 constant vramwrite  \ mode low, /csw low, /csr high, /reset high

\ these constands define memory access for VDP
$800 constant screenbase
40 constant chars/l
24 constant lines/s
chars/l lines/s * constant screensize

( delay calibration: "45 drop" takes 30us. "latest drop" takes 25us. )
( delay100us is actually 99.5us all in )
: delay37us ;
: delay100us base @ drop sp@ drop ;

: setdatawrite $ff ddrb c! ;
: setdataread $00 ddrb c! ;

: initvia $ff ddra c! $ff ddrb c! inactive control c! ;

( 45 drop here is just to generate a brief pause )
: initvdp
    inactive control c!   reset control c!
    45 drop 45 drop   inactive control c! ;

( assert a value on the control lines briefly )
: strobe ( signal -- )
   control c! 45 drop 45 drop inactive control c! ;

: reg! ( value reg# -- )
   $80 + swap
   setdatawrite data c! regwrite strobe setdataread
   setdatawrite data c! regwrite strobe setdataread ;

: reg@
    inactive control c!
    regread control c!
    setdataread
    data c@
    setdatawrite
    inactive control c!
  ;

: initreg0
   0 0 reg!  0 1 reg!  0 2 reg!  0 3 reg!
   0 4 reg!  0 5 reg!  0 6 reg!  0 7 reg! ;

: setcolor ( color -- )  7 reg! ;

: clearvram
   setdatawrite
   0 data c! regwrite strobe
   $40 data c! regwrite strobe
   16384 0 do 0 data c! vramwrite strobe loop ;

: fillvram
   setdatawrite
   0 data c! regwrite strobe
   $40 data c! regwrite strobe
   8192 0 do
     $aa data c! vramwrite strobe
     $55 data c! vramwrite strobe
 loop ;

: getstatus
   setdataread regread control c!  data c@ inactive control c!
   setdatawrite ;

( write data to the address set up on the TMS's auto-increment  )
( address register. repeated calls will write sequential bytes. )
: v+!  ( byte -- )
  setdatawrite
    data c!                 \ assert the data
    modelow control c!      \ MODE must go low first
    vramwrite control c!    \ then CSW
    modelow control c!      \ keep MODE low and others high
  ;

( write byte to designated VRAM address )
: v! ( byte vaddr -- )
  \ first, set up the address
  dup 8 rshift 63 and 64 or swap 255 and  \ set up address
  setdatawrite
  inactive control c!    \ set MODE high first
  data c!   regwrite strobe
  data c!   regwrite strobe
  \ brief pause
  45 drop 45 drop
  \ then write the byte
  v+! ;

( do a read from the pre-set address. repeated reads will read )
( sequentially via the TMS's auto-increment address register   )
: v+@  ( -- byte )
  setdataread
  \ there may be up to 8us between each read request but forth
  \ takes 10us between words so no need for a pause here.
  modelow control c!     \ MODE must go low first
  vramread control c!
  data c@
  modelow control c!     \ raise CSR again but leave MODE low
  ;

( read a byte from a specified VRAM address )
: v@  ( vaddr -- byte )
  \ first set up the address
  dup 8 rshift 63 and swap 255 and
  setdatawrite
  inactive control c!      \ set MODE high first
  data c!   regwrite strobe
  data c!   regwrite strobe
  \ address set up takes up to 2us. there's about 10us between
  \ forth words so we don't need a pause here.
  \ pause anyway?
  45 drop 45 drop
  \ then do the read
  v+@ ;


: read512 ( vaddr -- )
  v@ .  \ fetch the first byte
  \ now fetch more by strobing the control lines
  setdataread
  511 0 do vramread control c! data c@ . loop
  setdatawrite ;

: delay 0 do loop ;

: colorcycle
    16 0 do i dup 4 lshift or 7 reg! 20000 delay loop ;

: colortest
    20 0 do i . colorcycle loop ;

1 constant black
2 constant green
3 constant lightgreen
4 constant darkblue
5 constant lightblue
6 constant darkred
7 constant cyan
8 constant red
9 constant lightred
10 constant darkyellow
11 constant lightyellow
12 constant darkgreen
13 constant magenta
14 constant gray
15 constant white

( set text mode. also 16k. )
: text  2 0 reg! $D0 1 reg! 2 2 reg! ;

: mode1 0 0 reg! $C0 1 reg!  ;

: mode2 2 0 reg! $C0 1 reg!  ;

: $byte. hex 3 .r decimal ;

: $word. hex 4 .r decimal ;

: vdumpline cr dup $word. ." :" 16 0 do dup i + v@ $byte. loop drop ;
: vdump 16 / 0 do dup i 16 * + vdumpline loop drop ;

: blackwhite $f1 7 reg! ;
: bluewhite $f4 7 reg! ;
: redwhite $f6 7 reg! ;
: whiteblack $1f 7 reg! ;

: startvideo initvia initvdp initreg0 bluewhite text  ;

( set all chars on top row to same value )
( value -- )
: settoprow
    screenbase swap chars/l swap fill ;

( set all chars on designated row to same value )
( value row -- )
: setrown
    chars/l *
    dup screenbase + chars/l + swap
    screenbase +
    do dup i v! loop drop ;

: fillscreen ( byte -- )
    screensize screenbase + screenbase do
      dup i v! loop drop ;

: clearscreen 0 fillscreen ;

: nwrite 0 do 0 $800 v! loop ;

: writeall 16384 0 do dup v+! loop drop ;

: read256 256 0 do v+@ . loop ;

( set up basic font of just four simple characters )
: setupfont
   \ char 0 is space
   0 0 v!  0 1 v!  0 2 v!  0 3 v!  0 4 v!  0 5 v!  0 6 v!  0 7 v!

   \ char 1 is empty square
   $00 8 v!  $78 9 v!  $47 10 v!  $47 11 v!
   $78 12 v!  0 13 v!  0 14 v!  0 15 v!

   \ char 2 is fillde square
   $00 16 v!  $78 17 v!  $78 18 v!  $78 19 v!
   $78 20 v!  0 21 v!  0 22 v!  0 23 v!

   \ char 3 is cross
   $00 24 v!  $20 25 v!  $20 26 v!  $78 27 v!
   $20 28 v!  $20 29 v!  $00 30 v!  $00 31 v!
 ;

