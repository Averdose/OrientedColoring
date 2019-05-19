import pandas as pd

import seaborn as sns
import matplotlib.pyplot as plt
from matplotlib import style

IMG_PATH = './../target'
BruteMs = 'BruteMs'
BruteChromatic = 'BruteChromatic'
SLMs = 'SLMs'
SLChromatic = 'SLChromatic'
BFSMs = 'BFSMs'
BFSChromatic = 'BFSChromatic'
DSMs = 'DSMs'
DSChromatic = 'DSChromatic'

N = 'N'
SATUR = 'satur'


def plot_average_chromatic(df, include_brute=True, file_prefix=""):

    if include_brute:
        df[BruteChromatic].plot(kind='line', label='Algorytm dokładny')
    df[BFSChromatic].plot(kind='line', label='BFS')
    df[DSChromatic].plot(kind='line', label='DSatur')
    df[SLChromatic].plot(kind='line', label='Smallest last')

    plt.legend(loc='best')
    # plt.title("Czas wykonania programu algorytmem brute force")
    plt.ylabel("Liczba wykorzystanych kolorów")
    plt.xlabel("Liczba wierzchołków")

    plt.xticks(df.index, df[N].values)

    plt.savefig('{}/{}.png'.format(IMG_PATH, file_prefix+"average_chromatic"))
    # plt.savefig('{}/{}.pdf'.format(IMG_PATH, file_prefix+"average_chromatic"), format='pdf', dpi=1000)
    # plt.show()
    plt.close()


def main():
    style.use('ggplot')

    df = pd.read_csv("./average__results.csv", sep=';')

    up_to_8_df = df[(df[N] <= 8)]
    sparse_up_to_8 = up_to_8_df[(up_to_8_df[SATUR] == 0.1)]
    medium_up_to_8 = up_to_8_df[(up_to_8_df[SATUR] == 0.5)]
    dense_up_to_8 = up_to_8_df[(up_to_8_df[SATUR] == 0.9)]

    # plot_average_brute_execs(up_to_8_df)
    # plot_average_aprox_times(medium_up_to_8)
    # plot_average_chromatic(medium_up_to_8)

    from_9_df = df[(df[N] > 8)]
    sparse_from_9 = from_9_df[(from_9_df[SATUR] == 0.1)]
    medium_from_9 = from_9_df[(from_9_df[SATUR] == 0.5)]
    dense_from_9 = from_9_df[(from_9_df[SATUR] == 0.9)]

    plot_average_aprox_times(sparse_from_9, "big_sparse_")
    plot_average_aprox_times(medium_from_9, "big_medium_")
    plot_average_aprox_times(dense_from_9, "big_dense_")
    plot_average_chromatic(sparse_from_9, False, "big_sparse_")
    plot_average_chromatic(medium_from_9, False, "big_medium_")
    plot_average_chromatic(dense_from_9, False, "big_dense_")


    print('Finished program')


def line_plot_column(df, colname, title, ylabel, xlabel):
    df[colname].plot(kind='line')
    describe_plot_and_save(colname, title, ylabel, xlabel)


def hist_plot_column(df, colname, title, ylabel, xlabel, xticks, tick_labels):
    df[colname].plot(kind='hist', xticks=xticks)
    plt.xticks(xticks, tick_labels)
    describe_plot_and_save(colname, title, ylabel, xlabel)


def describe_plot_and_save(colname, title, ylabel, xlabel):
    plt.title(title)
    plt.ylabel(ylabel)
    plt.xlabel(xlabel)
    plt.savefig('{}/{}.png'.format(IMG_PATH, colname))
    plt.savefig('{}/{}.pdf'.format(IMG_PATH, colname), format='pdf', dpi=1000)
    plt.close()


def plot_average_brute_execs(up_to_8_df):
    sparse_up_to_8_df = up_to_8_df[(up_to_8_df[SATUR] == 0.1)]
    medium_up_to_8_df = up_to_8_df[(up_to_8_df[SATUR] == 0.5)]
    dense_up_to_8_df = up_to_8_df[(up_to_8_df[SATUR] == 0.9)]

    sparse_up_to_8_df[BruteMs].plot(kind='line', label='Grafy rzadkie')
    medium_up_to_8_df[BruteMs].plot(kind='line', label='Grafy wyważone')
    dense_up_to_8_df[BruteMs].plot(kind='line', label='Grafy gęste')

    plt.legend(loc='best')
    # plt.title("Czas wykonania programu algorytmem brute force")
    plt.ylabel("Czas wykonania (ms)")
    plt.xlabel("Liczba wierzchołków")

    plt.xticks(sparse_up_to_8_df.index, sparse_up_to_8_df[N].values)

    plt.savefig('{}/{}.png'.format(IMG_PATH, "brute_force_times"))
    # plt.savefig('{}/{}.pdf'.format(IMG_PATH, "brute_force_times"), format='pdf', dpi=1000)
    # plt.show()
    plt.close()


def plot_average_aprox_times(df, file_prefix=""):

    df[BFSMs].plot(kind='line', label='BFS')
    df[DSMs].plot(kind='line', label='DSatur')
    df[SLMs].plot(kind='line', label='Smallest last')

    plt.legend(loc='best')
    # plt.title("Czas wykonania programu algorytmem brute force")
    plt.ylabel("Czas wykonania (ms)")
    plt.xlabel("Liczba wierzchołków")

    plt.xticks(df.index, df[N].values)

    plt.savefig('{}/{}.png'.format(IMG_PATH, file_prefix+"aprox_times"))
    # plt.savefig('{}/{}.pdf'.format(IMG_PATH, file_prefix+"aprox_times"), format='pdf', dpi=1000)
    # plt.show()
    plt.close()

if __name__ == '__main__':
    main()
