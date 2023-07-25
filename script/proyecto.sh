#!/bin/bash
clear

run(){
    echo "\e[45m                                                                                                                       \e[49m

   __ _                 _                  _                                  _            
  /__(_) ___  ___ _   _| |_ __ _ _ __   __| | ___     /\/\   ___   ___   __ _| | ___       
 /_\ | |/ _ \/ __| | | | __/ _     _ \ / _  |/ _ \   /    \ / _ \ / _ \ / _  | |/ _ \      
//__ | |  __/ (__| |_| | || (_| | | | | (_| | (_) | / /\/\ \ (_) | (_) | (_| | |  __/_ _ _ 
\__/_/ |\___|\___|\__,_|\__\__,_|_| |_|\__,_|\___/  \/    \/\___/ \___/ \__, |_|\___(_|_|_)
   |__/                                                                 |___/              

\e[45m                                                                                                                       \e[49m"
                                            
    cd ../
    dotnet watch run --project MoogleServer
}

report(){
    clear 
    echo "\e[45m                                                                                                                       \e[49m


   ___                      _ _                 _           __ _    _____        __                                 
  / __\___  _ __ ___  _ __ (_) | __ _ _ __   __| | ___     /__\ |   \_   \_ __  / _| ___  _ __ _ __ ___   ___       
 / /  / _ \|  _   _ \|  _ \| | |/ _  |  _ \ / _  |/ _ \   /_\ | |    / /\/  _ \| |_ / _ \|  __|  _   _ \ / _ \      
/ /__| (_) | | | | | | |_) | | | (_| | | | | (_| | (_) | //__ | | /\/ /_ | | | |  _| (_) | |  | | | | | |  __/_ _ _ 
\____/\___/|_| |_| |_|  __/|_|_|\____|_| |_|\____|\___/  \__/ |_| \____/ |_| |_|_|  \___/|_|  |_| |_| |_|\___(_|_|_)
                     |_|                                                                                            
                                                                               

\e[45m                                                                                                                       \e[49m"
                                            
    cd ../
    cd Informe
    pdflatex -synctex=1 -interaction=nonstopmode Informe.tex
}

slides(){
    clear 
    echo "\e[45m                                                                                                                       \e[49m 

   ___                      _ _                 _            
  / __\___  _ __ ___  _ __ (_) | __ _ _ __   __| | ___       
 / /  / _ \| '_   _ \| '_ \| | |/ _  | '_ \ / _  |/ _ \      
/ /__| (_) | | | | | | |_) | | | (_| | | | | (_| | (_) | _ _ 
\____/\___/|_| |_| |_| .__/|_|_|\__,_|_| |_|\__,_|\___(_|_|_)
                     |_|                                     

                                            
\e[45m                                                                                                                       \e[49m"
    cd ../
    cd Presentacion
    pdflatex -synctex=1 -interaction=nonstopmode Presentacion.tex
}

show_report(){
    cd ..
    cd Informe
    archivo="Informe.pdf"

    if [ ! -e "$archivo" ]; then
        report
        xdg-open Informe.pdf
    else
        xdg-open Informe.pdf
    fi
}

show_report(){
    cd ..
    cd Presentacion
    archivo="Presentacion.pdf"

    if [ ! -e "$archivo" ]; then
        slides
        xdg-open Presentacion.pdf
    else
        xdg-open Presentacion.pdf
    fi
}

clean(){
    cd ../
    cd Informe
    rm -v Informe.aux
    rm -v Informe.log
    rm -v Informe.out
    rm -v Informe.pdf
    rm -v Informe.synctex.gz
    rm -v Informe.toc

    cd ../
    cd Presentacion
    rm -v Presentacion.aux
    rm -v Presentacion.log
    rm -v Presentacion.out
    rm -v Presentacion.pdf
    rm -v Presentacion.synctex.gz
    rm -v texput.log
}


if [ $1 = "run" ]; then
    clear
    run
fi

if [ $1 = "report" ]; then
    clear
    report
fi
if [ $1 = "slides" ]; then
    clear
    slides
fi
if [ $1 = "show_report" ]; then
    clear
    show_report
fi
if [ $1 = "show_slides" ]; then
    clear
    show_report
fi
if [ $1 = "clean" ]; then
    clear
    clean
fi


