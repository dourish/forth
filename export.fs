( copytext takes a block number and copies the text   )
( into a buffer for export to a file. Only one block  )
( at a time, and no text for end-of-buffer.           )

variable exportbuf 1024 allot

: copyline ( fromaddr toaddr -- toaddr2 )
   64 0 do 1 pick i + c@ dup >r
     1 pick c! 1+
     r> $0a = if leave then
     cr .s
   loop swap drop ;


: copytext ( buffer# address -- )
   swap block swap
   16 0 do >r dup r> copyline >r 64 + r> loop
   drop drop ;
